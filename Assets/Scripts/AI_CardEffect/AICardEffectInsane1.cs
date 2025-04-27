using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class AICardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] Choose a player and guess a number (cannot guess 1). If guessed correctly, target is eliminated.");

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
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        // Show guess UI
        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
            {
                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("Cannot guess number 1. Please choose again.");
                    return;
                }

                int targetValue = selectedTarget.GetHandValue();
                if (targetValue == guessedNumber)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"Correct! {selectedTarget.playerName} is eliminated!");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"Wrong guess. {selectedTarget.playerName}'s card was {targetValue}.");
                }

                currentPlayer.GoInsane();
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
                UIManager.Instance.Log($"Correct! {targetPlayers[randomIndex].playerName}'s card was {targetValue}. They are eliminated.");
                targetPlayers[randomIndex].Eliminate();
            }
            else
            {
                UIManager.Instance.Log($"Wrong guess. {targetPlayers[randomIndex].playerName}'s card was {targetValue}. Continue the game.");
            }

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] Choose a player. If their card is 1, they are immediately eliminated; otherwise, guess once.");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
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

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowGuessEffect(targets, (selectedTarget, guessedNumber) =>
            {
                int realValue = selectedTarget.GetHandValue();

                if (realValue == 1)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName}'s card is 1. Eliminated immediately!");
                }
                else
                {
                    if (guessedNumber == 1)
                    {
                        UIManager.Instance.ShowPopup("Cannot guess number 1. Please choose again.");
                        return;
                    }

                    if (guessedNumber == realValue)
                    {
                        selectedTarget.Eliminate();
                        UIManager.Instance.ShowPopup($"Correct! {selectedTarget.playerName} is eliminated!");
                    }
                    else
                    {
                        UIManager.Instance.ShowPopup($"Wrong guess. {selectedTarget.playerName}'s card was {realValue}.");
                    }
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI logic in insane mode
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            int realValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} selected {selectedTarget.playerName}.");

            if (realValue == 1)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName}'s card is 1. Eliminated immediately!");
            }
            else
            {
                int[] possibleGuesses = { 2, 3, 4, 5, 6, 7, 8 }; // Cannot guess 1
                int guessedNumber = possibleGuesses[Random.Range(0, possibleGuesses.Length)];

                UIManager.Instance.Log($"AI {currentPlayer.playerName} guessed {guessedNumber}.");

                if (guessedNumber == realValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"Correct! {selectedTarget.playerName} is eliminated!");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"Wrong guess. {selectedTarget.playerName}'s card was {realValue}.");
                }
            }

            GameManager.Instance.EndTurn();
        }
    }
}
