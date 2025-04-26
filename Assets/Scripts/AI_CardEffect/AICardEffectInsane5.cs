using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane5 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【正常效果】目标弃牌并抽一张");

        List<Player> targets = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有有效目标");
            GameManager.Instance.EndTurn();
            return;
        }

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                Card oldCard = selectedTarget.RemoveCard();
                if (oldCard != null)
                {
                    selectedTarget.DiscardCard(oldCard);
                }

                selectedTarget.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 弃牌并抽了一张新牌");

                currentPlayer.GoInsane(); // 正常效果后进入疯狂状态
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI 玩家随机选择目标
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            
            UIManager.Instance.Log($"AI {currentPlayer.playerName} 选择了 {selectedTarget.playerName} 作为目标");
            
            Card oldCard = selectedTarget.RemoveCard();
            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
                UIManager.Instance.Log($"{selectedTarget.playerName} 弃掉了 {oldCard.cardName}");
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);
            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 弃牌并抽了一张新牌");

            currentPlayer.GoInsane(); // 正常效果后进入疯狂状态
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】偷取目标 → 弃一张 → 给对方强制添加0号牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有有效目标");
            return;
        }

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                Card stolenCard = selectedTarget.RemoveCard();
                if (stolenCard == null)
                {
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌，偷牌失败");
                    GameManager.Instance.EndTurn();
                    return;
                }

                currentPlayer.AddCard(stolenCard);
                Debug.Log($"{currentPlayer.playerName} 偷取了 {selectedTarget.playerName} 的手牌：{stolenCard.cardName}");

                List<Card> myCards = currentPlayer.GetCards();
                if (myCards.Count == 2)
                {
                    UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                    {
                        currentPlayer.DiscardCard(cardToDiscard);

                        // 发给目标 0 号牌
                        GameManager.Instance.GiveSpecificCardToPlayer(selectedTarget, "0");

                        // 展示 UI 提示目标获得 0 号牌
                        UIManager.Instance.ShowMiGoBrainReveal(selectedTarget, () =>
                        {
                            GameManager.Instance.EndTurn();
                        });
                    });
                }
                else
                {
                    Debug.LogWarning("当前玩家手牌数量不足两张，无法执行弃牌操作");
                    GameManager.Instance.EndTurn();
                }
            });
        }
        else
        {
            // AI 玩家随机选择目标
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            
            UIManager.Instance.Log($"AI {currentPlayer.playerName} 选择了 {selectedTarget.playerName} 作为目标");
            
            Card stolenCard = selectedTarget.RemoveCard();
            if (stolenCard == null)
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌，偷牌失败");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(stolenCard);
            UIManager.Instance.Log($"{currentPlayer.playerName} 偷取了 {selectedTarget.playerName} 的手牌：{stolenCard.cardName}");

            List<Card> myCards = currentPlayer.GetCards();
            if (myCards.Count == 2)
            {
                // AI 随机选择要弃掉的牌
                Card cardToDiscard = myCards[Random.Range(0, myCards.Count)];
                currentPlayer.DiscardCard(cardToDiscard);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 弃掉了 {cardToDiscard.cardName}");

                // 发给目标 0 号牌
                GameManager.Instance.GiveSpecificCardToPlayer(selectedTarget, "0");
                UIManager.Instance.Log($"{selectedTarget.playerName} 获得了一张 0 号牌");
                
                // AI 不需要展示 UI，直接结束回合
                GameManager.Instance.EndTurn();
            }
            else
            {
                Debug.LogWarning("当前玩家手牌数量不足两张，无法执行弃牌操作");
                GameManager.Instance.EndTurn();
            }
        }
    }
}