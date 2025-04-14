using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int PlayerIndex { get; private set; }  
    public string playerName;
    bool isAlive = true; // Player Status
    private int winRounds = 0; // Player Win Round
    private int winRoundsInsane = 0; // Player Win Round Insane
    bool isInsane = false; // Player Insane Status

    private Hand hand = new Hand();

    public void DrawCard(Deck deck)
    {
        
        Card newCard = deck.Draw();
            if (newCard != null)
            {
                hand.AddCard(newCard);
                Debug.Log($"{PlayerIndex} draw a card: {newCard.cardName}");
            
        }
    }

    public void PlayCard(Card card)
    {       
            hand.playCard(card);
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
        int randomIndex = Random.Range(0, hand.CardCount);
        PlayCard(hand.GetCards()[randomIndex]);
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
