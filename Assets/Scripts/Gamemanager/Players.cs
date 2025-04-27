using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Transform[] cardSlots = new Transform[2]; // Card slots
    public HandUI handUI;
    public int PlayerIndex { get; private set; }
    public string playerName;
    private bool isAlive = true; // Player status
    public bool isHuman = false; // Player type
    private int winRounds = 0; // Normal win rounds
    private int winRoundsInsane = 0; // Insane win rounds
    private bool isInsane = false; // Insane status
    private bool isProtected = false; // Protected status (effect of Card 4)
    private bool isImmortalThisRound = false; // Immortal this round (effect of insane Card 4)
    private List<Card> discardedCards = new List<Card>();
    private Hand hand;

    public Hand Hand => hand;

    private void Awake()
    {
        hand = new Hand(handUI); // Pass handUI reference
    }

    public bool IsHuman()
    {
        return isHuman;
    }

    public void DrawCard(Deck deck)
    {
        Card newCard = deck.Draw();
        if (newCard == null)
        {
            Debug.LogWarning($"{playerName} can't draw because the deck is empty.");
            return;
        }

        hand.AddCard(newCard);
        Debug.Log($"{PlayerIndex} drew card: {newCard.cardName}");

        CardController controller = newCard.cardObject?.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetCardOwner(this);
        }

        if (handUI != null && isHuman)
        {
            handUI.UpdateHandUI(hand.GetCards());
        }
    }

    public void StealCard(Card newCard)
    {
        hand.AddCard(newCard);
        Debug.Log($"{PlayerIndex} steal card: {newCard.cardName}");

        CardController controller = newCard.cardObject?.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetCardOwner(this);
        }

        if (handUI != null && isHuman)
        {
            handUI.UpdateHandUI(hand.GetCards());
        }
    }


    public void PlayCard(Card card)
    {
        hand.PlayCard(card);
        NoticeController.Instance.SetNotice($"{playerName} plays {card.cardName}");

    }

    public void WinRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
            Debug.Log($"{playerName} won an insane round. Total insane wins: {winRoundsInsane}");
        }
        else
        {
            winRounds++;
            Debug.Log($"{playerName} won a normal round. Total wins: {winRounds}");
        }
    }

    public void RandomPlayCard()
    {
        int randomIndex = Random.Range(0, hand.CardCount);
        PlayCard(hand.GetCards()[randomIndex]);
    }

    public void Initialize(int index, string name)
    {
        PlayerIndex = index;
        playerName = name;
    }

    public void Reset()
    {
        hand.ClearHand();
        isAlive = true;
        isInsane = false;
    }

    public int GetHandValue()
    {
        return hand.GetCardValue();
    }

    public int CheckWin()
    {
        return winRounds;
    }

    public int CheckInsanityWin()
    {
        return winRoundsInsane;
    }

    public void Eliminate()
    {
        if (isImmortalThisRound)
        {
            return;
        }
        Debug.Log($"{playerName} is eliminated.");
        isAlive = false;

        if (isHuman && GameManager.Instance != null && !GameManager.Instance.IsGameEnded())
        {
            Debug.Log("Human player eliminated. Triggering defeat UI.");
            GameManager.Instance.TriggerPlayerDefeat($"{playerName} has been eliminated!");
        }
    }


    public bool IsAlive()
    {
        return isAlive;
    }

    public List<Card> GetCards()
    {
        return hand.GetCards();
    }

    public void SetProtected(bool status)
    {
        isProtected = status;
    }

    public bool IsProtected()
    {
        return isProtected;
    }

    public void DiscardCard(Card card)
    {
        hand.Discard(card);
        RecordDiscard(card);

        // If discarding an 8-value card -> immediate elimination
        if (!IsInsane() && card.value == 8 && card.isInsane)
        {
            Eliminate();
        }
        else if (card.value == 8 && !card.isInsane)
        {
            Eliminate();
        }
    }

    public Card RemoveCard()
    {
        List<Card> cards = hand.GetCards();
        if (cards.Count > 0)
        {
            Card cardToReturn = cards[0];
            hand.ClearHand(); // Only one card, clear hand
            return cardToReturn;
        }
        return null;
    }

    public void AddCard(Card card)
    {
        hand.AddCard(card);
    }

    public bool IsInsane()
    {
        return isInsane;
    }

    public void GoInsane()
    {
        isInsane = true;
    }

    public void SetImmortalThisRound(bool status)
    {
        isImmortalThisRound = status;
    }

    public bool IsImmortal()
    {
        return isImmortalThisRound;
    }

    public void RecordDiscard(Card card)
    {
        if (card != null)
        {
            discardedCards.Add(card);
            Debug.Log($"{playerName} discarded card: {card.cardName}");

            if (!IsImmortal() && !IsInsane() && card.value == 8 && card.isInsane)
            {
                Eliminate();
            }
            else if (!IsImmortal() && card.value == 8 && !card.isInsane)
            {
                Eliminate();
            }

            if (!IsImmortal() && card.value == 0)
            {
                Eliminate();
            }
        }
    }

    public int CountInsaneDiscards()
    {
        return discardedCards.FindAll(c => c.isInsane).Count;
    }

    public void ClearDiscardHistory()
    {
        discardedCards.Clear();
    }

    public Card GetOtherCard(Card excludedCard)
    {
        List<Card> cards = GetCards();
        foreach (Card card in cards)
        {
            if (card != excludedCard)
            {
                return card;
            }
        }
        return null;
    }

    public bool RevealAndDiscardTopCards(Deck deck)
    {
        List<Card> topCards = deck.PeekTopCards(CountInsaneDiscards());
        bool foundInsane = false;

        foreach (Card card in topCards)
        {
            if (card == null) continue;

            if (card.isInsane)
            {
                foundInsane = true;
            }

            RecordDiscard(card);
        }

        deck.RemoveTopCards(topCards.Count);

        return foundInsane;
    }

    public Card GetSelectedCard()
    {
        return hand.GetSelectedCard();
    }
}
