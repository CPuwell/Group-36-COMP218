using UnityEngine;
using System.Collections.Generic;

public class CardEffect1 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // ��ȡ���������ڳ���ң��ų��Լ� + �ѳ��֣�
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);


        if (targetPlayers.Count == 0)
        {
            Debug.Log("û��������ҿɹ�ѡ��");
            return;
        }

        // TODO: �滻Ϊ UI ѡ�˺�����
        Player selectedTarget = targetPlayers[2]; // ��ʱ��ѡ��2��
        int guessedNumber = 5; // ��ʱ�²�����Ϊ5

        if (guessedNumber == 1)
        {
            Debug.Log("���ܲ����� 1��������ѡ��");
            return;
        }

        int targetValue = selectedTarget.GetHandValue();

        if (targetValue == guessedNumber)
        {
            Debug.Log($"�����ˣ�{selectedTarget.playerName} �������� {targetValue}���������ˣ�");
            selectedTarget.Eliminate();
        }
        else
        {
            Debug.Log($"�´��ˡ�{selectedTarget.playerName} �������� {targetValue}��������Ϸ");
        }

        // �غϽ����� GameManager ����
        GameManager.Instance.EndTurn();
    }
}
