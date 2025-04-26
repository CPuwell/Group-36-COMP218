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
    private List<Card> discardedCards = new List<Card>();// ���ƶ�

    private Hand hand = new Hand(); // ��Ȼ˽�б���


    public Hand Hand => hand;       // �ⲿͨ�����ֻ�����Է���

    public bool IsHuman()
    {
        return isHuman;
    }
    public void DrawCard(Deck deck)
    {
        Card newCard = deck.Draw();
        if (newCard == null)
        {
            Debug.LogWarning($"{playerName} ����ʧ�ܣ�deck ��");
            return;
        }

        hand.AddCard(newCard);
        Debug.Log($"{PlayerIndex} �鵽��һ���ƣ�{newCard.cardName}");

        // ���ù���
        CardController controller = newCard.cardObject.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetCardOwner(this);
        }

        //  AI ������� return������ִ�� UI
        if (!isHuman)
        {
            Debug.Log($"{playerName} �� AI������ UI ��ʾ");
            return;
        }

        //  ������Ҳ�ִ������ UI ����
        for (int i = 0; i < cardSlots.Length; i++)
        {
            
                Debug.Log($"cardSlot[{i}] = {cardSlots[i]}"); //  ��ӡ��
               

            if (cardSlots[i] == null)
            {
                Debug.LogError($"[{playerName}] cardSlots[{i}] is null��");
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
            Debug.Log($"{playerName} �����࣬�������� UI");


            if (isHuman) { 
                Debug.Log($"{playerName} �����࣬�������� UI");
                handUI.UpdateHandUI(hand.GetCards());
            }
        }
    }





    public void PlayCard(Card card)
    {       
        hand.PlayCard(card);
        Debug.Log($"{playerName} ���ƣ�{card.cardName}");
        if (handUI != null)
        {
            handUI.UpdateHandUI(hand.GetCards());
        }
    }

    public void WinRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
            Debug.Log($"{playerName} Ӯ���˷��غϣ���ǰ����ʤ�غ�����{winRoundsInsane}");
        } else
        {
            winRounds++;
            Debug.Log($"{playerName} Ӯ���˻غϣ���ǰ���ǻ�ʤ�غ�����{winRounds}");
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


    //������zjs�ӵ�
    public void Eliminate()
    {
        if (isImmortalThisRound)
        {
            return;
        }
        Debug.Log($"{playerName} ����̭�ˣ�");
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
