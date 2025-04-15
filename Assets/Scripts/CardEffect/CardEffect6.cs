using UnityEngine;
using System.Collections.Generic;

public class CardEffect6 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // �ҳ���ѡĿ�꣨�������Լ������ܱ�������������ţ�
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            Debug.Log("û�п��Խ��������");
            return;
        }

        // TODO�������滻Ϊ UI ѡ��
        Player targetPlayer = targetPlayers[2];

        Card myCard = currentPlayer.RemoveCard();
        Card theirCard = targetPlayer.RemoveCard();

        if (myCard == null || theirCard == null)
        {
            Debug.Log("����ʧ�ܣ����������û������");
            // ����������ֽ��������Լ����߼������� return
            return;
        }

        currentPlayer.AddCard(theirCard);
        targetPlayer.AddCard(myCard);

        Debug.Log($"{currentPlayer.playerName} �� {targetPlayer.playerName} ����������");
        Debug.Log($"{currentPlayer.playerName} ���ڳ��У�{theirCard.cardName}");
        Debug.Log($"{targetPlayer.playerName} ���ڳ��У�{myCard.cardName}");

        GameManager.Instance.EndTurn();
    }
}
