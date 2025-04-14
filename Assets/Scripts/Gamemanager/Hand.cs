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
    private void PlayCard(Card card)
    {
        if (cards.Contains(card))
        {
            card.PlayCard();  // ��������㿨�ƵĲ����߼�
            cards.Remove(card);
            selectedCard = null;
            Debug.Log($"Played card: {card.cardName}");
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
}
