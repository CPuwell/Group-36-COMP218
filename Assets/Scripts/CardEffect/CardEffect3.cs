using UnityEngine;
using System.Collections.Generic;

public class CardEffect3 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // ��ȡ����������
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            Debug.Log("û��������ҿ���ѡ��");
            return;
        }

        // TODO���滻Ϊ UI ѡ��
        Player selectedTarget = targetPlayers[2]; // ��ʱѡһ�����

        int currentValue = currentPlayer.GetHandValue();
        int targetValue = selectedTarget.GetHandValue();

        Debug.Log($"{currentPlayer.playerName} �������� {currentValue}");
        Debug.Log($"{selectedTarget.playerName} �������� {targetValue}");

        if (currentValue > targetValue)
        {
            selectedTarget.Eliminate();
            Debug.Log($"{currentPlayer.playerName} Ӯ�ˣ�{selectedTarget.playerName} ���֣�");
        }
        else if (currentValue < targetValue)
        {
            currentPlayer.Eliminate();
            Debug.Log($"{selectedTarget.playerName} Ӯ�ˣ�{currentPlayer.playerName} ���֣�");
        }
        else
        {
            Debug.Log("ƽ�֣�û�˳���");
        }

        GameManager.Instance.EndTurn();
    }
}
