using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane6 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] Swap hands with a target player.");

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
            UIManager.Instance.ShowPlayerSelectionSimple(targets, target =>
            {
                Card myCard = currentPlayer.RemoveCard();
                Card theirCard = target.RemoveCard();

                if (myCard != null && theirCard != null)
                {
                    currentPlayer.AddCard(theirCard);
                    target.AddCard(myCard);

                    Debug.Log($"{currentPlayer.playerName} swapped hands with {target.playerName}.");
                    UIManager.Instance.ShowPopup($"{currentPlayer.playerName} successfully swapped hands with {target.playerName}!");
                }
                else
                {
                    Debug.Log("Swap failed. One of the players has no cards.");
                    UIManager.Instance.ShowPopup("Swap failed. One of the players has no cards.");
                }

                currentPlayer.GoInsane();
                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player randomly selects a target
            Player target = targets[Random.Range(0, targets.Count)];

            UIManager.Instance.Log($"AI {currentPlayer.playerName} selected {target.playerName} as the target.");

            Card myCard = currentPlayer.RemoveCard();
            Card theirCard = target.RemoveCard();

            if (myCard != null && theirCard != null)
            {
                currentPlayer.AddCard(theirCard);
                target.AddCard(myCard);

                UIManager.Instance.Log($"AI {currentPlayer.playerName} swapped hands with {target.playerName}.");
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} successfully swapped hands with {target.playerName}!");
            }
            else
            {
                UIManager.Instance.Log("Swap failed. One of the players has no cards.");
                UIManager.Instance.ShowPopup("Swap failed. One of the players has no cards.");
            }

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] Redistribute all players' cards.");

        List<Player> targets = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);
        List<Player> playersWithCards = targets.FindAll(p => p.GetCards().Count > 0);

        if (playersWithCards.Count < 2)
        {
            UIManager.Instance.ShowPopup("Not enough players with cards to redistribute! (At least two required)");
            GameManager.Instance.EndTurn();
            return;
        }

        // Collect all cards
        List<Card> collectedCards = new List<Card>();
        foreach (Player player in playersWithCards)
        {
            Card card = player.RemoveCard();
            if (card != null)
            {
                collectedCards.Add(card);
                Debug.Log($"Collected {player.playerName}'s card: {card.cardName}");
                if (currentPlayer.isHuman)
                {
                    UIManager.Instance.Log($"Collected {player.playerName}'s card.");
                }
            }
        }

        Shuffle(collectedCards);

        // Redistribute the cards
        for (int i = 0; i < playersWithCards.Count; i++)
        {
            playersWithCards[i].AddCard(collectedCards[i]);
            Debug.Log($"{playersWithCards[i].playerName} received a new card: {collectedCards[i].cardName}");
            if (currentPlayer.isHuman)
            {
                UIManager.Instance.Log($"{playersWithCards[i].playerName} received a new card.");
            }
        }

        // Simple notification popup
        UIManager.Instance.ShowPopup("All players' hands have been redistributed.");

        GameManager.Instance.EndTurn();
    }

    private void Shuffle(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
