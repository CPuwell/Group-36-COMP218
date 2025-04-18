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
    public void PlayCard(Card card)
    {
        if (!CanPlayCard(card))// 判断有没有7号牌的影响
        {
            return;
        }

        if (cards.Contains(card))
        {
            card.PlayCard();
            cards.Remove(card);
            selectedCard = null;

            // 出牌后记录进入弃牌堆
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
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

    //添加限制规则来判断有7号牌时另一张可不可以打
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

    //弃牌
    public void Discard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"弃牌：{card.cardName}");
        }
    }

}
