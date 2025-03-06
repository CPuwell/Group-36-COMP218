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

    // 移除一张牌（出牌）
    public void RemoveCard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"Draw: {card.CardName}");
        }
    }

    // 获取所有手牌
    public List<Card> GetCards()
    {
        return new List<Card>(cards); // 返回一个副本，防止外部修改原始列表
    }

    // 选择一张牌进行出牌（这里暂时用第 0 张卡牌）
    public Card ChooseCardToPlay()
    {
        if (cards.Count > 0)
        {
            return cards[0]; // 选择第一张牌出牌（可以优化成 AI 选择逻辑）
        }
        return null;
    }

    // 清空手牌（用于新一轮）
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
