using UnityEngine;
using System.Collections.Generic;

public class CardEffect2 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ��鿴");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard(); // �������װ�������
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // ѡ��һ�����
        UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, selectedPlayer =>
        {
            UIManager.Instance.Log($"{currentPlayer.playerName} �鿴�� {selectedPlayer.playerName} ������");

            List<Card> cards = selectedPlayer.GetCards();

            if (cards.Count > 0)
            {
                Card card = cards[0];

                // չʾ����ͼ��
                UIManager.Instance.ShowCardReveal(card, selectedPlayer.playerName, () =>
                {
                    GameManager.Instance.EndTurn(); // չʾ������غ�
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedPlayer.playerName} û������");
                GameManager.Instance.EndTurn();
            }
        });
    }
}
