using UnityEngine;
using System.Collections.Generic;

public class CardEffect5 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // 包括自己，且活着、未受保护
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

        // 使用“允许自己”的选人面板
        UIManager.Instance.ShowPlayerSelectionAllowSelf(targetPlayers, selectedTarget =>
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
