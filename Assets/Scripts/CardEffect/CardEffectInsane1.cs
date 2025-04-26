using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】选择玩家并猜牌（不能猜1），猜中出局");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");

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

        // 弹出猜牌 UI
        UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
        {
            if (guessedNumber == 1)
            {
                UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                return;
            }

            int targetValue = selectedTarget.GetHandValue();
            if (targetValue == guessedNumber)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"猜中！{selectedTarget.playerName} 出局！");
            }
            else
            {
                UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}");
            }

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】选一名玩家，若其手牌为1直接淘汰，否则猜一次");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowGuessEffect(targets, (selectedTarget, guessedNumber) =>
        {
            int realValue = selectedTarget.GetHandValue();

            if (realValue == 1)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 手牌是 1，直接出局！");
            }
            else
            {
                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                    return;
                }

                if (guessedNumber == realValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"猜中了！{selectedTarget.playerName} 出局！");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {realValue}");
                }
            }

            GameManager.Instance.EndTurn();
        });
    }

}
