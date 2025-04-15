using UnityEngine;
using System.Collections.Generic;

public class CardEffect2 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // ��ȡ����������
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);


        if (targetPlayers.Count == 0)
        {
            Debug.Log("û��������ҿɹ��鿴");
            return;
        }

        // TODO: ������ UI �滻
        Player selectedTarget = targetPlayers[2]; // ��ʱѡ��һ�����


        // ��ȡĿ���������
        List<Card> targetCards = selectedTarget.GetCards();

        if (targetCards.Count > 0)
        {
            Debug.Log($"{selectedTarget.playerName} �������ǣ�{targetCards[0].cardName}");
        }
        else
        {
            Debug.Log($"{selectedTarget.playerName} û������");
        }

        GameManager.Instance.EndTurn();
    }
}
