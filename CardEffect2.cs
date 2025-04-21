using UnityEngine;
using System.Collections.Generic;

public class CardEffect2 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");
            return;
        }

        // 调用 UI 流程
        UIManager.Instance.ShowCardReveal(targetPlayers, (revealedPlayer) =>
        {
            // 可选：日志记录
            UIManager.Instance.Log($"{currentPlayer.playerName} 查看了 {revealedPlayer.playerName} 的手牌");

            // 回合结束
            GameManager.Instance.EndTurn();
        });
    }
}
