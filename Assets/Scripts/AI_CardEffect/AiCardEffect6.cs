using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect6 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("No available targets.");
            // Discard the selected card
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // Use the simplified player selection UI
        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, targetPlayer =>
            {
                Card myCard = currentPlayer.RemoveCard();
                Card theirCard = targetPlayer.RemoveCard();

                if (myCard == null || theirCard == null)
                {
                    UIManager.Instance.ShowPopup("Swap failed: One of the players has no cards.");
                    GameManager.Instance.EndTurn();
                    return;
                }

                currentPlayer.AddCard(theirCard);
                targetPlayer.AddCard(myCard);

                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} successfully swapped hands with {targetPlayer.playerName}!");
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, targetPlayers.Count);

            Card myCard = currentPlayer.RemoveCard();
            Card theirCard = targetPlayers[randomIndex].RemoveCard();

            if (myCard == null || theirCard == null)
            {
                UIManager.Instance.ShowPopup("Swap failed: One of the players has no cards.");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(theirCard);
            targetPlayers[randomIndex].AddCard(myCard);

            UIManager.Instance.ShowPopup($"{currentPlayer.playerName} successfully swapped hands with {targetPlayers[randomIndex].playerName}!");
            GameManager.Instance.EndTurn();
        }
    }
}
