using UnityEngine;
using System.Collections.Generic;

public class CardEffect5 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // ��ȡ���п��Ա�ѡ�е���ң������Լ���
        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targetPlayers.Count == 0)
        {
            Debug.Log("û����ҿ��Ա�ѡ��");
            return;
        }

        // TODO: �� UI �����ѡ��Ŀ��
        Player selectedTarget = targetPlayers[2];

        selectedTarget.DiscardHand();
        selectedTarget.DrawCard(GameManager.Instance.deck);

        Debug.Log($"{selectedTarget.playerName} ���Ʋ�����һ������");

        GameManager.Instance.EndTurn();
    }
}
