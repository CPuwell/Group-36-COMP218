using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect1 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("No available players to chose.");
            // Discard the selected card
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // If player is human, open guess panel
        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
            {
                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("You cannot guess number 1. Please choose another number.");
                    return;
                }

                int targetValue = selectedTarget.GetHandValue();

                if (targetValue == guessedNumber)
                {
                    UIManager.Instance.ShowPopup($"Correct guess! {selectedTarget.playerName}'s card was {targetValue}.");
                    selectedTarget.Eliminate();
                }
                else
                {
                    UIManager.Instance.ShowPopup($"Wrong guess. Continue the game.");
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, targetPlayers.Count);

            int[] options = { 0, 2, 3, 4, 5, 6, 7, 8 };
            int guessedNumber = options[UnityEngine.Random.Range(0, options.Length)];
            int targetValue = targetPlayers[randomIndex].GetHandValue();

            if (targetValue == guessedNumber)
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} guessed {guessedNumber}. Correct! {targetPlayers[randomIndex].playerName} is eliminated!");
                targetPlayers[randomIndex].Eliminate();
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} guessed the card of{targetPlayers[randomIndex].playerName} is {guessedNumber}. Wrong guess.");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
