using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane5 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] Target discards a card and draws a new one.");

        List<Player> targets = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

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
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                Card oldCard = selectedTarget.RemoveCard();
                if (oldCard != null)
                {
                    selectedTarget.DiscardCard(oldCard);
                }

                selectedTarget.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} discarded a card and drew a new one.");

                currentPlayer.GoInsane(); // After sane effect, go insane
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player randomly selects a target
            Player selectedTarget = targets[Random.Range(0, targets.Count)];

            UIManager.Instance.Log($"AI {currentPlayer.playerName} selected {selectedTarget.playerName} as the target.");

            Card oldCard = selectedTarget.RemoveCard();
            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
                UIManager.Instance.Log($"{selectedTarget.playerName} discarded {oldCard.cardName}.");
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);
            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} discarded a card and drew a new one.");

            currentPlayer.GoInsane(); // After sane effect, go insane
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] Steal a card from target -> discard one -> give target a Card 0.");

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
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                Card stolenCard = selectedTarget.RemoveCard();
                if (stolenCard == null)
                {
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards. Steal failed.");
                    GameManager.Instance.EndTurn();
                    return;
                }

                currentPlayer.StealCard(stolenCard);
                Debug.Log($"{currentPlayer.playerName} stole {selectedTarget.playerName}'s card: {stolenCard.cardName}");

                List<Card> myCards = currentPlayer.GetCards();
                if (myCards.Count == 2)
                {
                    UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                    {
                        currentPlayer.DiscardCard(cardToDiscard);

                        // Give target a Card 0
                        GameManager.Instance.GiveSpecificCardToPlayer(selectedTarget, "0");

                        // Show Mi-Go reveal UI
                        UIManager.Instance.ShowMiGoBrainReveal(selectedTarget, () =>
                        {
                            GameManager.Instance.EndTurn();
                        });
                    });
                }
                else
                {
                    Debug.LogWarning("Current player does not have two cards. Cannot discard.");
                    GameManager.Instance.EndTurn();
                }
            });
        }
        else
        {
            // AI player randomly selects a target
            Player selectedTarget = targets[Random.Range(0, targets.Count)];

            UIManager.Instance.Log($"AI {currentPlayer.playerName} selected {selectedTarget.playerName} as the target.");

            Card stolenCard = selectedTarget.RemoveCard();
            if (stolenCard == null)
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards. Steal failed.");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(stolenCard);
            UIManager.Instance.Log($"{currentPlayer.playerName} stole {selectedTarget.playerName}'s card: {stolenCard.cardName}");

            List<Card> myCards = currentPlayer.GetCards();
            if (myCards.Count == 2)
            {
                // AI randomly discards a card
                Card cardToDiscard = myCards[Random.Range(0, myCards.Count)];
                currentPlayer.DiscardCard(cardToDiscard);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} discarded {cardToDiscard.cardName}.");

                // Give target a Card 0
                GameManager.Instance.GiveSpecificCardToPlayer(selectedTarget, "0");
                UIManager.Instance.Log($"{selectedTarget.playerName} received a Card 0.");

                // No need for AI to show Mi-Go UI
                GameManager.Instance.EndTurn();
            }
            else
            {
                Debug.LogWarning("Current player does not have two cards. Cannot discard.");
                GameManager.Instance.EndTurn();
            }
        }
    }
}
