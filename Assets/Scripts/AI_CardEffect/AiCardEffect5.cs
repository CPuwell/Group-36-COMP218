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
            UIManager.Instance.ShowPopup("无法选择");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard(); 
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        if(currentPlayer.isHuman == true){
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
    }else{
        if (targetPlayers.Count> 0)
            {
                int randomIndex = UnityEngine.Random.Range(0,targetPlayers.Count);
                int selectedTarget = targetPlayers[randomIndex];
            }
    
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
    };
    }
}
