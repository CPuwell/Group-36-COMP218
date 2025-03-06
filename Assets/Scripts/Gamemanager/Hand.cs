using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards = new List<Card>(); 

    public int CardCount => cards.Count; 

    public void AddCard(Card card)
    {
        cards.Add();
    }

    // �Ƴ�һ���ƣ����ƣ�
    public void RemoveCard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"Draw: {card.CardName}");
        }
    }

    // ��ȡ��������
    public List<Card> GetCards()
    {
        return new List<Card>(cards); // ����һ����������ֹ�ⲿ�޸�ԭʼ�б�
    }

    // ѡ��һ���ƽ��г��ƣ�������ʱ�õ� 0 �ſ��ƣ�
    public Card ChooseCardToPlay()
    {
        if (cards.Count > 0)
        {
            return cards[0]; // ѡ���һ���Ƴ��ƣ������Ż��� AI ѡ���߼���
        }
        return null;
    }

    // ������ƣ�������һ�֣�
    public void ClearHand()
    {
        cards.Clear();
        Debug.Log("Clear Hands");
    }

    public int GetCardValue()
    {
        return cards[0].value;
    }
}
