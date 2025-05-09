using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class CardEffectInsane5 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】目标弃牌并抽一张");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("无法选择玩家");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, selectedTarget =>
        {
            Card oldCard = selectedTarget.RemoveCard();
            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);

            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 弃牌并抽了一张新牌");

            currentPlayer.GoInsane(); // 理智效果后进入疯狂状态
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】偷取目标 → 弃一张 → 给对方强制添加0号牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("无法选择玩家");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            GameManager.Instance.EndTurn();
            return;
        }

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
}
