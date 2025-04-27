using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane3 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] Compare hand values with a target player. The lower value is eliminated.");

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
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        if (currentPlayer.isHuman)
        {
            // UI selection
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                int currentValue = currentPlayer.GetHandValue();
                int targetValue = selectedTarget.GetHandValue();

                UIManager.Instance.Log($"{currentPlayer.playerName}'s hand value: {currentValue}");
                UIManager.Instance.Log($"{selectedTarget.playerName}'s hand value: {targetValue}");

                if (currentValue > targetValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, {selectedTarget.playerName} is eliminated.");
                }
                else if (currentValue < targetValue)
                {
                    currentPlayer.Eliminate();
                    UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, {currentPlayer.playerName} is eliminated.");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, No one is eliminated.");
                }

                currentPlayer.GoInsane(); // After effect, go insane
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player logic - random selection
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} compares with {selectedTarget.playerName}.");
            UIManager.Instance.Log($"{currentPlayer.playerName}'s hand value: {currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName}'s hand value: {targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, {selectedTarget.playerName} is eliminated.");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, {currentPlayer.playerName} is eliminated.");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} compare the card with {selectedTarget.playerName}, No one is eliminated.");
            }

            currentPlayer.GoInsane(); // After effect, go insane
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] If the target is not insane, they are eliminated. Otherwise, they are immune.");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
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

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                if (!selectedTarget.IsInsane())
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} is not insane and has been eliminated!");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} is already insane and is immune to elimination.");
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player insane effect logic - random selection
            Player selectedTarget = targets[Random.Range(0, targets.Count)];

            UIManager.Instance.Log($"AI {currentPlayer.playerName} selected {selectedTarget.playerName}.");

            if (!selectedTarget.IsInsane())
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} choose the {selectedTarget.playerName}, {selectedTarget.playerName} is not insane and has been eliminated!");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} choose the {selectedTarget.playerName}, {selectedTarget.playerName} is already insane and is immune to elimination.");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
