using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("ִ�С����ǡ�Ч�������ƣ�������Ŀ����֣����ܲ�1");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            Debug.Log("û�п���ѡ������");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��
        int guess = 5; // TODO: �滻Ϊ UI ���루����Ϊ1��

        if (guess == 1)
        {
            Debug.Log("���ܲ�1���ƣ�");
            return;
        }

        if (target.GetHandValue() == guess)
        {
            target.Eliminate();
            Debug.Log($"���У�{target.playerName} ����");
        }
        else
        {
            Debug.Log("�´��ˣ�û��Ч��");
        }

        // ����Ч��ִ���꣬���� insane ״̬
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("ִ�С����Ч�����ȼ��Է��Ƿ���1���ƣ������ٲ�һ��");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            Debug.Log("û�п���ѡ������");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��

        int handValue = target.GetHandValue();
        if (handValue == 1)
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} ������1�ţ�ֱ�ӳ��֣�");
        }
        else
        {
            Debug.Log($"{target.playerName} ����1���ƣ������ٲ�һ��");

            int guess = 6; // TODO: �滻Ϊ UI ���루����Ϊ1��

            if (guess == 1)
            {
                Debug.Log("���ܲ�1���ƣ�");
                return;
            }

            if (handValue == guess)
            {
                target.Eliminate();
                Debug.Log($"���У�{target.playerName} ����");
            }
            else
            {
                Debug.Log("�ٴβ´��ˣ�û��Ч��");
            }
        }
        GameManager.Instance.EndTurn();
    }
}
