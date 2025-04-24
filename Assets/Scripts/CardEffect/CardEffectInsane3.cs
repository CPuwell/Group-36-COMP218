using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane3 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч������Ŀ����ұ��ƣ��������߳���");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û�п��ԶԾ���Ŀ�����");
            return;
        }

        // UI ѡ��
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} ��������ֵ��{currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} ��������ֵ��{targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} ���֣������ϵͣ�");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} ���֣������ϵͣ�");
            }
            else
            {
                UIManager.Instance.ShowPopup("ƽ�֣����˳���");
            }

            currentPlayer.GoInsane(); // ����Ч���������״̬
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч������Ŀ��δ��� �� ��̭��������ʾ����");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û�п���ָ����Ŀ�����");
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            if (!selectedTarget.IsInsane())
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} δ������״̬������̭��");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} �Ѵ��ڷ��״̬��������̭");
            }

            GameManager.Instance.EndTurn();
        });
    }
}
