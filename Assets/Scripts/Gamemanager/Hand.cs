using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards = new List<Card>();
    private Card selectedCard = null;

    public int CardCount => cards.Count;

    public void AddCard(Card card)
    {
        if (card != null)
        {
            cards.Add(card);
            Debug.Log($"�������: {card.cardName}");
        }
    }

    public void SelectCard(Card card)
    {
        if (selectedCard == card)
        {
            PlayCard(card);
        }
        else
        {
            selectedCard = card;
            Debug.Log($"ѡ�п���: {card.cardName}");
        }
    }

    public void PlayCard(Card card)
    {
        if (card == null || card.cardObject == null)
        {
            Debug.LogWarning("��ͼ��һ���Ѿ������ٵĿ���");
            return;
        }


        if (!CanPlayCard(card))
            return;

        if (cards.Contains(card))
        {
            // �����Ŀ��ƴ����߼� prefab��Ч������
            if (card.effectPrefab != null)
            {
                GameObject.Instantiate(card.effectPrefab);
            }

            // ������õ��� CardController ���ƶ�������Ч
            CardController controller = card.cardObject.GetComponent<CardController>();
            if (controller != null)
            {
                controller.Play();
            }

            cards.Remove(card);
            selectedCard = null;

            Debug.Log($"���ƣ�{card.cardName}");

            // ֪ͨ GameManager ��¼����
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
        }
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards); // ���ظ�������ֹ�ⲿ�޸�
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

    public void Discard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"����: {card.cardName}");
        }
    }
}
