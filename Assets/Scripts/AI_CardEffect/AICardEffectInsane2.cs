using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane2 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] View one player's hand.");

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
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                List<Card> cards = selectedTarget.GetCards();
                if (cards.Count > 0)
                {
                    UIManager.Instance.ShowCardReveal(cards[0], selectedTarget.playerName, () =>
                    {
                        currentPlayer.GoInsane(); // After viewing, go insane
                        GameManager.Instance.EndTurn();
                    });
                }
                else
                {
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards.");
                    currentPlayer.GoInsane();
                    GameManager.Instance.EndTurn();
                }
            });
        }
        else
        {
            // AI player logic
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            List<Card> cards = selectedTarget.GetCards();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} viewed {selectedTarget.playerName}'s hand.");

            if (cards.Count > 0)
            {
                // Log the card name for tracking
                UIManager.Instance.Log($"AI {currentPlayer.playerName} saw {selectedTarget.playerName}'s card: {cards[0].cardName}");
                currentPlayer.GoInsane();
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards.");
                currentPlayer.GoInsane();
            }

            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] View target's hand + draw a card + discard one card.");

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
                List<Card> targetCards = selectedTarget.GetCards();

                if (targetCards.Count > 0)
                {
                    UIManager.Instance.ShowCardReveal(targetCards[0], selectedTarget.playerName, () =>
                    {
                        currentPlayer.DrawCard(GameManager.Instance.deck);

                        List<Card> myCards = currentPlayer.GetCards();
                        if (myCards.Count == 2)
                        {
                            // Let human player choose a card to discard
                            UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                            {
                                currentPlayer.DiscardCard(cardToDiscard);
                                GameManager.Instance.EndTurn();
                            });
                        }
                        else
                        {
                            Debug.Log("Player does not have two cards. Cannot discard.");
                            GameManager.Instance.EndTurn();
                        }
                    });
                }
                else
                {
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards.");
                    currentPlayer.DrawCard(GameManager.Instance.deck);
                    GameManager.Instance.EndTurn();
                }
            });
        }
        else
        {
            // AI logic for insane mode
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            List<Card> targetCards = selectedTarget.GetCards();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} viewed {selectedTarget.playerName}'s hand.");

            if (targetCards.Count > 0)
            {
                UIManager.Instance.Log($"AI {currentPlayer.playerName} saw {selectedTarget.playerName}'s card: {targetCards[0].cardName}");

                currentPlayer.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} drew a card.");

                List<Card> myCards = currentPlayer.GetCards();
                if (myCards.Count == 2)
                {
                    Card cardToDiscard = myCards[Random.Range(0, myCards.Count)];
                    currentPlayer.DiscardCard(cardToDiscard);
                    UIManager.Instance.Log($"AI {currentPlayer.playerName} discarded {cardToDiscard.cardName}.");
                }
                else
                {
                    Debug.Log("AI does not have two cards. Cannot discard.");
                }
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} has no cards.");
                currentPlayer.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} drew a card.");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
