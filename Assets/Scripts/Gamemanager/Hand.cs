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
<<<<<<< Updated upstream
        if (!CanPlayCard(card)) // �ж���û�� 7 ���Ƶ�����Ӱ��
        {
=======
        if (!CanPlayCard(card))
>>>>>>> Stashed changes
            return;

        if (cards.Contains(card))
        {
<<<<<<< Updated upstream
            // ʹ�� CardController ����ͳһ�����߼�
            CardController controller = card.cardObject.GetComponent<CardController>();
            if (controller != null)
            {
                controller.Play();
            }
            else
            {
                Debug.LogWarning("CardController δ�����ڿ��� GameObject �ϣ�");
            }

            cards.Remove(card);
            selectedCard = null;

            // ���Ƽ�¼�Ա���
=======
            cards.Remove(card);
            selectedCard = null;

            Debug.Log($"���ƣ�{card.cardName}");

            // ��������Ч����ͨ�� prefab ʵ������
            if (card.effectPrefab != null)
            {
                GameObject.Instantiate(card.effectPrefab);
            }

            // ֪ͨ GameManager ��¼����
>>>>>>> Stashed changes
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
        }
    }

<<<<<<< Updated upstream


    public List<Card> GetCards()
=======
    public List<CardData> GetCards()
>>>>>>> Stashed changes
    {
        return new List<CardData>(cards); // ���ظ���
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
            CardData other = cards.Find(c => c != card);
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
