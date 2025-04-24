using UnityEngine;
using System.Collections.Generic;

public class CardEffect6 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可以交换的玩家");
            return;
        }

        // 使用统一的简化选择界面
        UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, targetPlayer =>
        {
            Card myCard = currentPlayer.RemoveCard();
            Card theirCard = targetPlayer.RemoveCard();

            if (myCard == null || theirCard == null)
            {
                UIManager.Instance.ShowPopup("交换失败：其中有玩家没有手牌");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(theirCard);
            targetPlayer.AddCard(myCard);

            UIManager.Instance.Log($"{currentPlayer.playerName} 与 {targetPlayer.playerName} 交换了手牌");
            UIManager.Instance.Log($"{currentPlayer.playerName} 现在持有：{theirCard.cardName}");
            UIManager.Instance.Log($"{targetPlayer.playerName} 现在持有：{myCard.cardName}");

            UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 与 {targetPlayer.playerName} 成功交换了手牌！");
            GameManager.Instance.EndTurn();
        });
    }
}
