using System.Collections.Generic;
using UnityEngine;

public class RuleBasedAIPlayer : Player
{
    public void PlayTurn()
    {
        List<Card> hand = GetCards();

        if (hand.Count == 0)
        {
            Debug.LogWarning($"{playerName} has no cards to play.");
            return;
        }

        // 选一张合法的牌来打出
        Card selectedCard = ChooseValidCard(hand);

        if (selectedCard != null)
        {
            PlayCard(selectedCard);
            Debug.Log($"{playerName} plays {selectedCard.cardName}");
        }
        else
        {
            // 如果找不到合法牌（理论上不太可能），随机出一张
            Debug.LogWarning($"{playerName} no valid card found, playing randomly.");
            PlayCard(hand[0]);
        }
    }

    private Card ChooseValidCard(List<Card> hand)
    {
        hand.Sort((a, b) => a.value.CompareTo(b.value)); // 按牌面数值排序，优先出小牌

        foreach (var card in hand)
        {
            if (IsValidPlay(card))
            {
                return card;
            }
        }

        return null; // 如果没有找到
    }

    private bool IsValidPlay(Card card)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(this);
        List<int> handValues = new List<int>();
        foreach (var c in GetCards())
        {
            handValues.Add(c.value);
        }

        bool has0 = handValues.Contains(0);
        bool has8 = handValues.Contains(8);

        switch (card.value)
        {   
            case 0:
                return false;
            case 1: // 猜牌
                return true;
            case 2: // 比较大小
                return true;
            case 3: // 查看手牌
                return true;
            case 5: // 让别人弃牌
                if (targets.Count == 0)
                    return false;
                break;
                else
                {
                    return true;
                }

            case 6:
                return true;

            case 7: 
                return true;
                

            case 8: 
                if (GetCards().Count > 1)
                    return false;
                break;

            default:
                break;
        }

        return true; // 其他默认可以出
    }
}
