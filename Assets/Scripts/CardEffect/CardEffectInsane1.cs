using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч����ѡ����Ҳ����ƣ����ܲ�1�������г���");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ��鿴");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();

            return;
        }

        // �������� UI
        UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
        {
            if (guessedNumber == 1)
            {
                UIManager.Instance.ShowPopup("���ܲ� 1��������ѡ��");
                return;
            }

            int targetValue = selectedTarget.GetHandValue();
            if (targetValue == guessedNumber)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"���У�{selectedTarget.playerName} ���֣�");
            }
            else
            {
                UIManager.Instance.ShowPopup($"�´��ˡ�{selectedTarget.playerName} �������� {targetValue}");
            }

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����ѡһ����ң���������Ϊ1ֱ����̭�������һ��");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ��鿴");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowGuessEffect(targets, (selectedTarget, guessedNumber) =>
        {
            int realValue = selectedTarget.GetHandValue();

            if (realValue == 1)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} ������ 1��ֱ�ӳ��֣�");
            }
            else
            {
                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("���ܲ� 1��������ѡ��");
                    return;
                }

                if (guessedNumber == realValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"�����ˣ�{selectedTarget.playerName} ���֣�");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"�´��ˡ�{selectedTarget.playerName} �������� {realValue}");
                }
            }

            GameManager.Instance.EndTurn();
        });
    }

}
