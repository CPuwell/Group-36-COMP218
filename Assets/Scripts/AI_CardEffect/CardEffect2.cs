using UnityEngine;
using System.Collections.Generic;

public class CardEffect2 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");
             // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard(); 
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // ѡ��һ�����
        if(currentPlayer.isHuman == true){
        UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, selectedPlayer =>
        {
            UIManager.Instance.Log($"{currentPlayer.playerName} 查看了 {selectedPlayer.playerName} 的手牌");

            List<Card> cards = selectedPlayer.GetCards();

            if (cards.Count > 0)
            {
                Card card = cards[0];

                // չʾ����ͼ��
                UIManager.Instance.ShowCardReveal(card, selectedPlayer.playerName, () =>
                {
                    GameManager.Instance.EndTurn(); // չʾ������غ�
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedPlayer.playerName} 没有手牌");
                GameManager.Instance.EndTurn();
            }
        })
        }else{
            return;
     
     };
    }
}
