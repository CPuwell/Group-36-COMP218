using UnityEngine;
using System.Collections.Generic;

public class CardEffect6 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("û�п��Խ��������");
            return;
        }

        // ʹ��ͳһ�ļ�ѡ�����
        UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, targetPlayer =>
        {
            Card myCard = currentPlayer.RemoveCard();
            Card theirCard = targetPlayer.RemoveCard();

            if (myCard == null || theirCard == null)
            {
                UIManager.Instance.ShowPopup("����ʧ�ܣ����������û������");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(theirCard);
            targetPlayer.AddCard(myCard);

            UIManager.Instance.ShowPopup($"{currentPlayer.playerName} �� {targetPlayer.playerName} �ɹ����������ƣ�");
            GameManager.Instance.EndTurn();
        });
    }
}
