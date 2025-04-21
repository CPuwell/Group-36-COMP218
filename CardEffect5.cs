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
            UIManager.Instance.ShowPopup("没有玩家可以被选择");
            GameManager.Instance.EndTurn();
            return;
        }

        // 使用 UIManager 弹出选择界面
        UIManager.Instance.ShowPlayerSelection(targetPlayers, selectedTarget =>
        {
            Card oldCard = selectedTarget.RemoveCard();

            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
                Debug.Log($"{selectedTarget.playerName} 弃掉了 {oldCard.cardName}");
            }
            else
            {
                Debug.Log($"{selectedTarget.playerName} 没有手牌可以弃掉");
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);

            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 弃掉了手牌并抽了一张新牌");
            GameManager.Instance.EndTurn();
        });
    }
}
