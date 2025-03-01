using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int PlayerIndex { get; private set; }  
    public string playerName;
    public List<Card> hand = new List<Card>();
    bool isAlive = true; // Player Status
    int winRound = 0; // Player Win Round

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
        return winRound;
    }

    public void winRound()
    {
        winRound++;
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
}
