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

        // ʹ�� UI ѡ��
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} ��������ֵ��{currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} ��������ֵ��{targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} ���֣������ͣ�");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} ���֣������ͣ�");
            }
            else
            {
                UIManager.Instance.ShowPopup("������ͬ�����˳���");
            }

            currentPlayer.GoInsane(); // ���Ǻ���
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
            var uiNotice = Object.FindFirstObjectByType<UIEliminationResult>(); // �Ƽ�д��

            if (!selectedTarget.IsInsane())
            {
                selectedTarget.Eliminate();
                uiNotice?.Show("This player is not in a crazy state and has been defeated and eliminated by you!");
            }
            else
            {
                uiNotice?.Show("The player has gone crazy and is immune to this attack.");
            }

            GameManager.Instance.EndTurn();
        });
    }

}
