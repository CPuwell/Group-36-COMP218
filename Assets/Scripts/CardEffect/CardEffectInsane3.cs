using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane3 : MonoBehaviour, InsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч������Ŀ����ұ��ƣ��������߳���");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("û�п��ԶԾ���Ŀ�����");
            return;
        }

        Player target = targets[0]; // TODO���滻Ϊ UI ѡ��

        int currentValue = currentPlayer.GetHandValue();
        int targetValue = target.GetHandValue();

        Debug.Log($"{currentPlayer.playerName} ��������ֵ��{currentValue}");
        Debug.Log($"{target.playerName} ��������ֵ��{targetValue}");

        if (currentValue > targetValue)
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} ���֣������ͣ�");
        }
        else if (currentValue < targetValue)
        {
            currentPlayer.Eliminate();
            Debug.Log($"{currentPlayer.playerName} ���֣������ͣ�");
        }
        else
        {
            Debug.Log("������ͬ�����˳���");
        }

        currentPlayer.GoInsane(); // ����Ч��ִ�к����insane״̬
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч������Ŀ�������δ�����ֱ�ӳ���");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("û�п���ָ����Ŀ�����");
            return;
        }

        Player target = targets[0]; // TODO���滻Ϊ UI ѡ��

        if (!target.IsInsane())
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} �Դ�������״̬�������Ч����̭��");
        }
        else
        {
            Debug.Log($"{target.playerName} �Ѿ��Ƿ��״̬��û��Ч��");
        }
        GameManager.Instance.EndTurn();
    }
}
