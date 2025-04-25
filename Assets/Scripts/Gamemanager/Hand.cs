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
            Debug.Log($"添加手牌: {card.cardName}");
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
            Debug.Log($"选中卡牌: {card.cardName}");
        }
    }

    public void PlayCard(Card card)
    {
        if (card == null || card.cardObject == null)
        {
            Debug.LogWarning("试图出一张已经被销毁的卡！");
            return;
        }


        if (!CanPlayCard(card))
            return;

        if (cards.Contains(card))
        {
            // 如果你的卡牌带有逻辑 prefab（效果卡）
            if (card.effectPrefab != null)
            {
                GameObject.Instantiate(card.effectPrefab);
            }

            // 如果你用的是 CardController 控制动画或特效
            CardController controller = card.cardObject.GetComponent<CardController>();
            if (controller != null)
            {
                controller.Play();
            }

            cards.Remove(card);
            selectedCard = null;

            Debug.Log($"出牌：{card.cardName}");

            // 通知 GameManager 记录弃牌
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
        }
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards); // 返回副本，防止外部修改
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
                Debug.Log("你不能打出这张牌，因为你持有7号牌，且另一张牌大于4。你必须打出7号牌！");
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
            Debug.Log($"弃牌: {card.cardName}");
        }
    }
}
