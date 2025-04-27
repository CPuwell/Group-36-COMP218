using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect2 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("No available players to view.");
            // Discard the selected card
            Card cardToDiscard = currentPlayer.GetSelectedCard(); 
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // Choose a player to view
        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targetPlayers, selectedPlayer =>
            {
                UIManager.Instance.Log($"{currentPlayer.playerName} viewed {selectedPlayer.playerName}'s card.");

                List<Card> cards = selectedPlayer.GetCards();

                if (cards.Count > 0)
                {
                    Card card = cards[0];

                    // Show the selected player's card
                    UIManager.Instance.ShowCardReveal(card, selectedPlayer.playerName, () =>
                    {
                        GameManager.Instance.EndTurn(); // End turn after revealing
                    });
                }
                else
                {
                    UIManager.Instance.ShowPopup($"{selectedPlayer.playerName} has no cards.");
                    GameManager.Instance.EndTurn();
                }
            });
        }
        else
        {
            GameManager.Instance.EndTurn();
        }
    }
}
