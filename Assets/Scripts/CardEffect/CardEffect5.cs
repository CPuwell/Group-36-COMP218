using UnityEngine;
using System.Collections.Generic;

public class CardEffect5 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 获取所有可以被选中的玩家（包括自己）
        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targetPlayers.Count == 0)
        {
            Debug.Log("没有玩家可以被选择");
            return;
        }

        // TODO: 用 UI 让玩家选择目标
        Player selectedTarget = targetPlayers[2];

        selectedTarget.DiscardHand();
        selectedTarget.DrawCard(GameManager.Instance.deck);

        Debug.Log($"{selectedTarget.playerName} 弃牌并抽了一张新牌");

        GameManager.Instance.EndTurn();
    }
}
