using UnityEngine;
using System.Collections.Generic;

public class CardEffect6 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

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

        // 使用统一的简化选择界面
        if(currentPlayer.isHuman == true){
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

            UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 与 {targetPlayer.playerName} 成功交换了手牌！");
            GameManager.Instance.EndTurn();
        });
    }else{

         if (targetPlayers.Count> 0)
            {
                int randomIndex = UnityEngine.Random.Range(0,targetPlayers.Count);
                int selectedTarget = targetPlayers[randomIndex];
            }
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

            UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 与 {targetPlayer.playerName} 成功交换了手牌！");
            GameManager.Instance.EndTurn();
        

    }
    }
}
