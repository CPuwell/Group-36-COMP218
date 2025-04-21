using UnityEngine;
using System.Collections.Generic;

public class CardEffect3 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可以选择");
            return;
        }

        // UI 选择目标 → 比较数值 → 判定输赢
        UIManager.Instance.ShowDuelTargetSelection(targetPlayers, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} 的手牌是 {currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} 的手牌是 {targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 赢了！{selectedTarget.playerName} 出局！");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 赢了！{currentPlayer.playerName} 出局！");
            }
            else
            {
                UIManager.Instance.ShowPopup("平局！没人出局");
            }

            GameManager.Instance.EndTurn();
        });
    }
}
    