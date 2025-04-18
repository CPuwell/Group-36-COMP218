using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards = new List<Card>(); 
    private System.Random random = new System.Random(); 

    public bool HasCards => cards.Count > 0; 

    public Deck()
    {
        InitializeDeck();
        Shuffle(); 
    }
    
    private void InitializeDeck()
    {
        cards.Clear();
       
        
    }

    // ϴ��
    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int swapIndex = random.Next(cards.Count);
            Card temp = cards[i];
            cards[i] = cards[swapIndex];
            cards[swapIndex] = temp;
        }
        Debug.Log("The cards have been shuffled");
    }

    // ��һ����
    public Card Draw()
    {
        if (cards.Count > 0)
        {
            Card drawnCard = cards[0];
            cards.RemoveAt(0);
            Debug.Log($"Draw card: {drawnCard.cardName}");
            return drawnCard;
        }
        else
        {
            Debug.Log("deck is empty");
            return null;
        }
    }

    // ��һ���ƷŻ��ƶѣ���������ĳЩ���⿨��Ч����
    public void ReturnCard(Card card)
    {
        if (card != null)
        {
            cards.Add(card);
            Debug.Log($"Card: {card.cardName} is back into deck");
        }
    }

    public bool IsEmpty()
    {
        if (cards.Count == 0)
        {
            return true;
        }
        else
        {
            Debug.Log($"deck has {cards.Count} cards");
            return false;
        }
    }
}
