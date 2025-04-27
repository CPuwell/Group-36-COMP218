using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect5 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        // Include self; must be alive and not protected
        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

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

        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowPlayerSelectionAllowSelf(targetPlayers, selectedTarget =>
            {
                Card oldCard = selectedTarget.RemoveCard();

                if (oldCard != null)
                {
                    selectedTarget.DiscardCard(oldCard);
                    Debug.Log($"{selectedTarget.playerName} discarded {oldCard.cardName}.");
                }
                else
                {
                    Debug.Log($"{selectedTarget.playerName} had no cards to discard.");
                }

                selectedTarget.DrawCard(GameManager.Instance.deck);

                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} discarded a card and drew a new one.");
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, targetPlayers.Count);

            Card oldCard = targetPlayers[randomIndex].RemoveCard();

            if (oldCard != null)
            {
                targetPlayers[randomIndex].DiscardCard(oldCard);
                Debug.Log($"{targetPlayers[randomIndex].playerName} discarded {oldCard.cardName}.");
            }
            else
            {
                Debug.Log($"{targetPlayers[randomIndex].playerName} had no cards to discard.");
            }

            targetPlayers[randomIndex].DrawCard(GameManager.Instance.deck);

            UIManager.Instance.ShowPopup($"{targetPlayers[randomIndex].playerName} discarded a card and drew a new one.");
            GameManager.Instance.EndTurn();
        }
    }
}
