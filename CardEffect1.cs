using UnityEngine;
using System.Collections.Generic;

public class CardEffect1 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供选择");
            return;
        }

        // 调用 UIManager 选择玩家 + 猜数字
        UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
        {
            if (guessedNumber == 1)
            {
                UIManager.Instance.ShowPopup("不能猜数字 1，请重新选择");
                return;
            }

            int targetValue = selectedTarget.GetHandValue();

            if (targetValue == guessedNumber)
            {
                UIManager.Instance.Log($"猜中了！{selectedTarget.playerName} 的手牌是 {targetValue}，他出局了！");
                selectedTarget.Eliminate();
            }
            else
            {
                UIManager.Instance.Log($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}，继续游戏");
            }

            GameManager.Instance.EndTurn();
        });
    }
}
