using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards = new List<Card>();
    private Card selectedCard = null;

    public int CardCount => cards.Count;

    // 添加卡牌
    public void AddCard(Card card)
    {
        if (card != null)
        {
            cards.Add(card);
            Debug.Log($"Add card: {card.cardName}");
        }
    }

    // 玩家点击卡牌时调用这个
    public void SelectCard(Card card)
    {
        if (selectedCard == card)
        {
            PlayCard(card); // 第二次点击相同卡牌，出牌
        }
        else
        {
            selectedCard = card; // 第一次点击，选中
            Debug.Log($"Selected: {card.cardName}");
        }
    }

    // 出牌
    private void PlayCard(Card card)
    {
        if (cards.Contains(card))
        {
            card.PlayCard();  // 这里调用你卡牌的播放逻辑
            cards.Remove(card);
            selectedCard = null;
            Debug.Log($"Played card: {card.cardName}");
        }
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards); // 返回副本
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
