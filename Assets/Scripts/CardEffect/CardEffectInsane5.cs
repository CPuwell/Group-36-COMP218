using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane5 : MonoBehaviour, InsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】目标弃牌并抽一张");

        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targets.Count == 0)
        {
            Debug.Log("没有有效目标");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人

        List<Card> cards = target.GetCards();
        if (cards.Count > 0)
        {
            target.DiscardCard(cards[0]);
        }

        target.DrawCard(GameManager.Instance.deck);
        Debug.Log($"{target.playerName} 已弃牌并抽新牌");

        currentPlayer.GoInsane(); // 理智效果执行完后变为insane
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】偷取目标手牌 → 弃一张 → 强制给予0号牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有有效目标");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人

        Card stolenCard = target.RemoveCard();
        if (stolenCard != null)
        {
            currentPlayer.AddCard(stolenCard);
            Debug.Log($"{currentPlayer.playerName} 偷取了 {target.playerName} 的手牌：{stolenCard.cardName}");

            // 弃一张牌（当前默认玩家只有两张牌，可弃掉其中一张）
            List<Card> myCards = currentPlayer.GetCards();
            if (myCards.Count > 1)
            {
                // TODO：未来可用 UI 选择要弃哪张
                Card cardToDiscard = myCards[0];
                currentPlayer.DiscardCard(cardToDiscard);
                Debug.Log($"{currentPlayer.playerName} 弃掉了 {cardToDiscard.cardName}");

                // 给目标玩家强制添加0号牌
                GameManager.Instance.GiveSpecificCardToPlayer(target, "0");
            }
            else
            {
                Debug.Log($"{target.playerName} 没有手牌可偷，效果失败");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
