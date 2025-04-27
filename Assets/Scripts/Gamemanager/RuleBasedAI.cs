using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class RuleBasedAI
{
    // Main entry: Choose a card to play
    public static Card ChooseCard(Player player)
    {
        List<Card> hand = player.GetCards();

        if (hand.Count == 0)
        {
            Debug.LogWarning($"{player.playerName} has no cards to play.");
            return null;
        }

        // Select a valid card to play
        Card selectedCard = ChooseValidCard(player, hand);

        if (selectedCard != null)
        {
            Debug.Log($"{player.playerName} plays {selectedCard.cardName}");
            return selectedCard;
        }
        else
        {
            Debug.LogWarning($"{player.playerName} found no valid card, playing randomly.");
            return hand[0]; // If no valid card found, play the first one
        }
    }

    // Select a valid card based on priority rules
    private static Card ChooseValidCard(Player player, List<Card> hand)
    {
        hand.Sort((a, b) => a.value.CompareTo(b.value)); // Sort by ascending value

        foreach (var card in hand)
        {
            if (IsValidPlay(player, card))
            {
                return card;
            }
        }

        return null; // No valid card found
    }

    // Check if a card can be played
    private static bool IsValidPlay(Player player, Card card)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(player);
        List<int> handValues = new List<int>();

        foreach (var c in player.GetCards())
        {
            handValues.Add(c.value);
        }

        bool has0 = handValues.Contains(0);
        bool has7 = handValues.Contains(7);
        bool has8 = handValues.Contains(8);

        switch (card.value)
        {
            case 0:
                return false; // Special: Card 0 cannot be played actively
            case 1:
            case 2:
            case 3:
                return true; // Guess card, compare values, view hand
            case 5:
                return !has7; // Card 5 can be played unless holding 7
            case 6:
                return !has7; // Card 6 same as above
            case 7:
                return true; // Card 7 is always playable
            case 8:
                if (player.GetCards().Count > 1)
                    return false; // Avoid playing 8 when having two cards
                break;
            default:
                break;
        }

        return true; // Other cards are generally playable
    }
}
