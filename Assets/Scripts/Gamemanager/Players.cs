using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int PlayerIndex { get; private set; }  
    public string playerName;
    bool isAlive = true; // Player Status
    private int winRounds = 0; // Player Win Round
    private int winRoundsInsane = 0; // Player Win Round Insane
    bool isInsane = false; // Player Insane Status
    bool isProtected = false; // Effect of card 4
    bool isImmortalThisRound = false;// Effect of card insane 4
    private List<Card> discardedCards = new List<Card>();// ���ƶ�

    private Hand hand = new Hand();

    public void DrawCard(Deck deck)
    {
        
        Card newCard = deck.Draw();
            if (newCard != null)
            {
                hand.AddCard(newCard);
                Debug.Log($"{PlayerIndex} draw a card: {newCard.cardName}");
            
        }
    }

    public void PlayCard(Card card)
    {       
            hand.playCard(card);
    }

    public int checkWin()
    {
        return winRounds;
    }

    public void winRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
        } else
        {
            winRounds++;
        }
    }

    public void randomPlayCard()
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
        hand.Clear();
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


    //������zjs�ӵ�
    public void Eliminate()
    {
        if (isImmortalThisRound)
        {
            return;
        }

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

    private List<Card> discardedCards = new List<Card>();

    public void RecordDiscard(Card card)
    {
        if (card != null)
        {
            discardedCards.Add(card);
            Debug.Log($"{playerName} ���Ƽ�¼���£�{card.cardName}");

            // ����8���� �� ��������
            if (!IsImmortal() && !IsInsane() && card.value == 8 && card.isInsane)
            {
                Eliminate();
            }
            else if (!IsImmortal() && card.value == 8 && !card.isInsane)
            {
                Eliminate();
            }

            // ����0���� �� ��������
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
}
