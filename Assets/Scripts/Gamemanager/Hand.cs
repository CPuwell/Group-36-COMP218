using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards = new List<Card>();
    private Card selectedCard = null;

    public int CardCount => cards.Count;

    // ��ӿ���
    public void AddCard(Card card)
    {
        if (card != null)
        {
            cards.Add(card);
            Debug.Log($"Add card: {card.cardName}");
        }
    }

    // ��ҵ������ʱ�������
    public void SelectCard(Card card)
    {
        if (selectedCard == card)
        {
            PlayCard(card); // �ڶ��ε����ͬ���ƣ����� 
        }
        else
        {
            selectedCard = card; // ��һ�ε����ѡ��
            Debug.Log($"Selected: {card.cardName}");
        }
    }

    // ����
    public void PlayCard(Card card)
    {
        if (!CanPlayCard(card))// �ж���û��7���Ƶ�Ӱ��
        {
            return;
        }

        if (cards.Contains(card))
        {
            card.PlayCard();
            cards.Remove(card);
            selectedCard = null;

            // ���ƺ��¼�������ƶ�
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
        }
    }


    public List<Card> GetCards()
    {
        return new List<Card>(cards); // ���ظ���
    }

    public void ClearHand()
    {
        cards.Clear();
        selectedCard = null;
    }

    public int GetCardValue()
    {
        return cards.Count > 0 ? cards[0].value : 0;
    }

    //������ƹ������ж���7����ʱ��һ�ſɲ����Դ�
    public bool CanPlayCard(Card card)
    {
        if (cards.Count == 2 && cards.Exists(c => c.value == 7))
        {
            Card other = cards.Find(c => c != card);
            if (card.value != 7 && other.value > 4)
            {
                Debug.Log("�㲻�ܴ�������ƣ���Ϊ�����7���ƣ�����һ���ƴ���4���������7���ƣ�");
                return false;
            }
        }
        return true;
    }   

    //����
    public void Discard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"���ƣ�{card.cardName}");
        }
    }

}
