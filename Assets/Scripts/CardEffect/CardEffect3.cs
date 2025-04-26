using UnityEngine;
using System.Collections.Generic;

public class CardEffect3 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("�޷�ѡ��");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard(); // �������װ�������
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // ʹ��ͳһ��Ŀ��ѡ�񷽷�
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} �������� {currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} �������� {targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} Ӯ�ˣ�{selectedTarget.playerName} ���֣�");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} Ӯ�ˣ�{currentPlayer.playerName} ���֣�");
            }
            else
            {
                UIManager.Instance.ShowPopup("ƽ�֣�û�˳���");
            }

            GameManager.Instance.EndTurn();
        });
    }
}
