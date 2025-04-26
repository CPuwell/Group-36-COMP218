using System.Collections.Generic;
using UnityEngine;

public static class RuleBasedAI
{
    // 主入口：选择一张牌来出
    public static Card ChooseCard(Player player)
    {
        List<Card> hand = player.GetCards();

        if (hand.Count == 0)
        {
            Debug.LogWarning($"{player.playerName} has no cards to play.");
            return null;
        }

        // 选一张合法的牌来打出
        Card selectedCard = ChooseValidCard(player, hand);

        if (selectedCard != null)
        {
            Debug.Log($"{player.playerName} plays {selectedCard.cardName}");
            return selectedCard;
        }
        else
        {
            Debug.LogWarning($"{player.playerName} no valid card found, playing randomly.");
            return hand[0]; // 如果找不到合法牌，打第一张
        }
    }

    // 按优先规则选择合法出牌
    private static Card ChooseValidCard(Player player, List<Card> hand)
    {
        hand.Sort((a, b) => a.value.CompareTo(b.value)); // 按数值升序排序

        foreach (var card in hand)
        {
            if (IsValidPlay(player, card))
            {
                return card;
            }
        }

        return null; // 没找到合法牌
    }

    // 判断一张牌是否可以出
    private static bool IsValidPlay(Player player, Card card)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(player);
        List<int> handValues = new List<int>();

        foreach (var c in player.GetCards())
        {
            handValues.Add(c.value);
        }

        bool has0 = handValues.Contains(0);
        bool has8 = handValues.Contains(8);

        switch (card.value)
        {
            case 0:
                return false; // 特殊：0不能主动打出
            case 1:
            case 2:
            case 3:
                return true; // 猜牌、比较大小、查看手牌
            case 5:
                return targets.Count > 0; // 让别人弃牌必须有目标
            case 6:
            case 7:
                return true;
            case 8:
                if (player.GetCards().Count > 1)
                    return false; // 有两张牌时尽量不打8
                break;
            default:
                break;
        }

        return true; // 其他默认可以出
    }
}

