using UnityEngine;
using System.Collections.Generic;

public class CardEffect6 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 找出可选目标（不能是自己、不能被保护、必须活着）
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            Debug.Log("没有可以交换的玩家");
            return;
        }

        // TODO：后期替换为 UI 选人
        Player targetPlayer = targetPlayers[2];

        Card myCard = currentPlayer.RemoveCard();
        Card theirCard = targetPlayer.RemoveCard();

        if (myCard == null || theirCard == null)
        {
            Debug.Log("交换失败，其中有玩家没有手牌");
            // 规则允许空手交换，可以继续逻辑；否则 return
            return;
        }

        currentPlayer.AddCard(theirCard);
        targetPlayer.AddCard(myCard);

        Debug.Log($"{currentPlayer.playerName} 与 {targetPlayer.playerName} 交换了手牌");
        Debug.Log($"{currentPlayer.playerName} 现在持有：{theirCard.cardName}");
        Debug.Log($"{targetPlayer.playerName} 现在持有：{myCard.cardName}");

        GameManager.Instance.EndTurn();
    }
}
