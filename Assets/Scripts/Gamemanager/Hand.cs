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
<<<<<<< Updated upstream
        if (!CanPlayCard(card)) // 判断有没有 7 号牌的限制影响
        {
=======
        if (!CanPlayCard(card))
>>>>>>> Stashed changes
            return;

        if (cards.Contains(card))
        {
<<<<<<< Updated upstream
            // 使用 CardController 调用统一出牌逻辑
            CardController controller = card.cardObject.GetComponent<CardController>();
            if (controller != null)
            {
                controller.Play();
            }
            else
            {
                Debug.LogWarning("CardController 未挂载在卡牌 GameObject 上！");
            }

            cards.Remove(card);
            selectedCard = null;

            // 弃牌记录仍保留
=======
            cards.Remove(card);
            selectedCard = null;

            Debug.Log($"出牌：{card.cardName}");

            // 触发卡牌效果（通过 prefab 实例化）
            if (card.effectPrefab != null)
            {
                GameObject.Instantiate(card.effectPrefab);
            }

            // 通知 GameManager 记录弃牌
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
        return new List<CardData>(cards); // 返回副本
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
