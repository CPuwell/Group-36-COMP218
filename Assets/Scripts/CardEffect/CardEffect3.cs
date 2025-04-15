using UnityEngine;
using System.Collections.Generic;

public class CardEffect3 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 获取其他存活玩家
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            Debug.Log("没有其他玩家可以选择");
            return;
        }

        // TODO：替换为 UI 选人
        Player selectedTarget = targetPlayers[2]; // 临时选一个玩家

        int currentValue = currentPlayer.GetHandValue();
        int targetValue = selectedTarget.GetHandValue();

        Debug.Log($"{currentPlayer.playerName} 的手牌是 {currentValue}");
        Debug.Log($"{selectedTarget.playerName} 的手牌是 {targetValue}");

        if (currentValue > targetValue)
        {
            selectedTarget.Eliminate();
            Debug.Log($"{currentPlayer.playerName} 赢了！{selectedTarget.playerName} 出局！");
        }
        else if (currentValue < targetValue)
        {
            currentPlayer.Eliminate();
            Debug.Log($"{selectedTarget.playerName} 赢了！{currentPlayer.playerName} 出局！");
        }
        else
        {
            Debug.Log("平局，没人出局");
        }

        GameManager.Instance.EndTurn();
    }
}
