using UnityEngine;
using System.Collections.Generic;

public class CardEffect5 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // �����Լ����һ��š�δ�ܱ���
        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targetPlayers.Count == 0)
        {
            Debug.Log("û����ҿ��Ա�ѡ��");
            UIManager.Instance.ShowPopup("û����ҿ��Ա�ѡ��");
            GameManager.Instance.EndTurn();
            return;
        }

        // ʹ�á������Լ�����ѡ�����
        UIManager.Instance.ShowPlayerSelectionAllowSelf(targetPlayers, selectedTarget =>
        {
            Card oldCard = selectedTarget.RemoveCard();

            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
                Debug.Log($"{selectedTarget.playerName} ������ {oldCard.cardName}");
            }
            else
            {
                Debug.Log($"{selectedTarget.playerName} û�����ƿ�������");
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);

            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} ���������Ʋ�����һ������");
            GameManager.Instance.EndTurn();
        });
    }
}
