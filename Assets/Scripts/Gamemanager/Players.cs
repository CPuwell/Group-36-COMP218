using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int PlayerIndex { get; private set; }  
    public string playerName;
    public List<Card> hand = new List<Card>();
    bool isAlive = true; // Player Status
    private int winRounds = 0; // Player Win Round
    private int winRoundsInsane = 0; // Player Win Round Insane
    bool isInsane = false; // Player Insane Status

    private Hand hand = new Hand();

    public void DrawCard(Deck deck)
    {
        
        Card newCard = deck.DrawCard();
            if (newCard != null)
            {
                hand.Add(newCard);
                Debug.Log($"{currentPlayerIndex} draw a card: {newCard.cardName}");
            }
    }

    public void PlayCard(Card card)
    {
        if (hand.Contains(card))
        {
            card.PlayCard(); 
            hand.Remove(card); 
        }
    }

    public int checkWin()
    {
        return winRounds;
    }

    public void winRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
        } else
        {
            winRounds++;
        }
    }

    public void randomPlayCard()
    {
        PlayCard(hand[randomIndex]);
    }

    public void Initialize(int index, string name)
    {
        PlayerIndex = index;
        playerName = name;
    }

    public void Reset()
    {
        hand.Clear();
        isAlive = true;
        isInsane = false;
    }

    public int GetHandValue()
    {
        return hand.GetCardValue();
    }

    public int CheckWin()
    {
        return winRounds;
    }

    public int CheckInsaintyWin()
    {
        return winRoundsInsane;
    }
}
