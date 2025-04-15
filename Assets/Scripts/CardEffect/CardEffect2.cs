using UnityEngine;
using System.Collections.Generic;

public class CardEffect2 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 获取其他存活玩家
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);


        if (targetPlayers.Count == 0)
        {
            Debug.Log("没有其他玩家可供查看");
            return;
        }

        // TODO: 后续用 UI 替换
        Player selectedTarget = targetPlayers[2]; // 临时选择一个玩家


        // 获取目标玩家手牌
        List<Card> targetCards = selectedTarget.GetCards();

        if (targetCards.Count > 0)
        {
            Debug.Log($"{selectedTarget.playerName} 的手牌是：{targetCards[0].cardName}");
        }
        else
        {
            Debug.Log($"{selectedTarget.playerName} 没有手牌");
        }

        GameManager.Instance.EndTurn();
    }
}
