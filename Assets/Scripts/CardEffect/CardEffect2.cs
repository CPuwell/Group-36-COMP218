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
            return;
        }

        // ���� UI ����
        UIManager.Instance.ShowCardReveal(targetPlayers, (revealedPlayer) =>
        {
            UIManager.Instance.Log($"{currentPlayer.playerName} �鿴�� {revealedPlayer.playerName} ������");
            GameManager.Instance.EndTurn();
        });
    }
}
