using UnityEngine;
using System.Collections.Generic;

public class CardEffect1 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 获取其他所有在场玩家（排除自己 + 已出局）
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);


        if (targetPlayers.Count == 0)
        {
            Debug.Log("没有其他玩家可供选择");
            return;
        }

        // TODO: 替换为 UI 选人和输入
        Player selectedTarget = targetPlayers[2]; // 临时先选择2号
        int guessedNumber = 5; // 临时猜测手牌为5

        if (guessedNumber == 1)
        {
            Debug.Log("不能猜数字 1，请重新选择");
            return;
        }

        int targetValue = selectedTarget.GetHandValue();

        if (targetValue == guessedNumber)
        {
            Debug.Log($"猜中了！{selectedTarget.playerName} 的手牌是 {targetValue}，他出局了！");
            selectedTarget.Eliminate();
        }
        else
        {
            Debug.Log($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}，继续游戏");
        }

        // 回合结束由 GameManager 控制
        GameManager.Instance.EndTurn();
    }
}
