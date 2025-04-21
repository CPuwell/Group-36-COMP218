using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane2 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】查看一名玩家的手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有可查看的目标玩家");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人

        List<Card> cards = target.GetCards();
        if (cards.Count > 0)
        {
            Debug.Log($"你查看了 {target.playerName} 的手牌：{cards[0].cardName}");
        }
        else
        {
            Debug.Log($"{target.playerName} 没有手牌");
        }

        currentPlayer.GoInsane(); // 理智效果结束后进入疯狂状态
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】查看目标玩家手牌 → 自己摸一张 → 弃一张");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有可查看的目标玩家");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人

        List<Card> targetCards = target.GetCards();
        if (targetCards.Count > 0)
        {
            Debug.Log($"你查看了 {target.playerName} 的手牌：{targetCards[0].cardName}");
        }
        else
        {
            Debug.Log($"{target.playerName} 没有手牌");
        }

        // 自己摸一张牌
        currentPlayer.DrawCard(GameManager.Instance.deck);

        // 弃一张牌（当前默认玩家只有两张牌，可弃掉其中一张）
        List<Card> myCards = currentPlayer.GetCards();
        if (myCards.Count > 1)
        {
            // TODO：未来可用 UI 选择要弃哪张
            Card cardToDiscard = myCards[0];
            currentPlayer.DiscardCard(cardToDiscard);
            Debug.Log($"{currentPlayer.playerName} 弃掉了 {cardToDiscard.cardName}");
        }
        else if (myCards.Count == 1)
        {
            // 特殊情况：本来没手牌，只抽了一张，那就不弃
            Debug.Log($"{currentPlayer.playerName} 只有一张牌，不执行弃牌");
        }
        GameManager.Instance.EndTurn();
    }
}