using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect3 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
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

        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                int currentValue = currentPlayer.GetHandValue();
                int targetValue = selectedTarget.GetHandValue();

                UIManager.Instance.Log($"{currentPlayer.playerName}'s card value: {currentValue}");
                UIManager.Instance.Log($"{selectedTarget.playerName}'s card value: {targetValue}");

                if (currentValue > targetValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{currentPlayer.playerName} wins! {selectedTarget.playerName}'s card value: {targetValue}. {selectedTarget.playerName} is eliminated.");
                }
                else if (currentValue < targetValue)
                {
                    currentPlayer.Eliminate();
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} wins! {selectedTarget.playerName}'s card value: {targetValue}. {currentPlayer.playerName} is eliminated.");
                }
                else
                {
                    UIManager.Instance.ShowPopup("It's a tie! No one is eliminated.");
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, targets.Count);
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = targets[randomIndex].GetHandValue();

            if (currentValue > targetValue)
            {
                targets[randomIndex].Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {targets[randomIndex].playerName}, {targets[randomIndex].playerName} is eliminated.");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {targets[randomIndex].playerName}, {currentPlayer.playerName} is eliminated.");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {targets[randomIndex].playerName}, No one is eliminated.");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
