using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Transform[] cardSlots = new Transform[2]; // �Ʋ�
    public HandUI handUI;
    public int PlayerIndex { get; private set; }  
    public string playerName;
    bool isAlive = true; // Player Status
    public bool isHuman = false; // Player Type
    private int winRounds = 0; // Player Win Round
    private int winRoundsInsane = 0; // Player Win Round Insane
    bool isInsane = false; // Player Insane Status
    bool isProtected = false; // Effect of card 4
    bool isImmortalThisRound = false;// Effect of card insane 4
    private List<Card> discardedCards = new List<Card>();

    private Hand hand;




    public Hand Hand => hand;

    private void Awake()
    {
        hand = new Hand(handUI); // 把 handUI 传进去！！
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
            Debug.LogWarning($"{playerName} can't draw because deck is empty");
            return;
        }

        hand.AddCard(newCard);
        Debug.Log($"{PlayerIndex} draw card:{newCard.cardName}");

        
        CardController controller = newCard.cardObject.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetCardOwner(this);
        }

       
        if (!isHuman)
        {
            
            return;
        }

        
        for (int i = 0; i < cardSlots.Length; i++)
        {
            
                Debug.Log($"cardSlot[{i}] = {cardSlots[i]}"); 
               

            if (cardSlots[i] == null)
            {
                Debug.LogError($"[{playerName}] cardSlots[{i}] is null");
                continue;
            }

            if (cardSlots[i].childCount == 0)
            {
                newCard.cardObject.transform.SetParent(cardSlots[i], false);
                newCard.cardObject.transform.localPosition = Vector3.zero;
                newCard.cardObject.transform.localScale = Vector3.one;

                CardUI cardUI = newCard.cardObject.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(newCard, this.hand);
                    cardUI.Flip(false);
                }

                break;
            }

            GameManager.RefreshDeckZone();
        }


        if (handUI != null)
        {
            

            if (isHuman) { 
                Debug.Log($"{playerName} Update hand UI");
                handUI.UpdateHandUI(hand.GetCards());
            }
        }
    }





    public void PlayCard(Card card)
    {       
        hand.PlayCard(card);
        Debug.Log($"{playerName} play card:{card.cardName}");
        
    }

    public void WinRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
            Debug.Log($"{playerName} win insane rounds:{winRoundsInsane}");
        } else
        {
            winRounds++;
            Debug.Log($"{playerName} win rounds:{winRounds}");
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

    public int CheckInsaintyWin()
    {
        return winRoundsInsane;
    }


    
    public void Eliminate()
    {
        if (isImmortalThisRound)
        {
            return;
        }
        Debug.Log($"{playerName} is eliminated");
        isAlive = false;
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

        // ����8���� �� ��������
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
            hand.ClearHand(); // ֻ��һ���ƣ�����ֱ�����
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
            Debug.Log($"{playerName} discarded card:{card.cardName}");

            
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

    public int CountInsaneDiscards()// �������ƶѷ��������
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
                foundInsane = true; // ���ַ����
            }

            // �ñ�׼�����߼������Ʒŵ����ƶ�
            RecordDiscard(card);
        }

        // ����Щ����ʽ���ƶ����Ƴ�
        deck.RemoveTopCards(topCards.Count);

        return foundInsane;
    }

    public Card GetSelectedCard()
    {
        return hand.GetSelectedCard();
    }




}
