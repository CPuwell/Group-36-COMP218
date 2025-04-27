using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private List<Card> cards = new List<Card>();
    private Card selectedCard = null;
    private HandUI handUI;
    public int CardCount => cards.Count;

    public void AddCard(Card card)
    {
        if (card != null)
        {
            cards.Add(card);
            Debug.Log($"Add Card: {card.cardName}");
        }
    }
    public Hand(HandUI handUI = null)
    {
        this.handUI = handUI;
    }

    public void SelectCard(Card card)
    {
        if (selectedCard == card)
        {
            PlayCard(card);
        }
        else
        {
            selectedCard = card;
            Debug.Log($"Chosen Card: {card.cardName}");
            
        }
    }

    public Card GetSelectedCard()
    {
        return selectedCard;
    }


    public void PlayCard(Card card)
    {
        if (card == null || card.cardObject == null)
        {
            Debug.LogWarning("Trying to play a card that has already been destroyed or is null.");
            return;
        }


        if (!CanPlayCard(card))
            return;

        if (cards.Contains(card))
        {
            

            // If you are using CardController to control animation or special effects
            CardController controller = card.cardObject.GetComponent<CardController>();
            GameManager.Instance.ShowCardInDiscardZone(card); // Show in discard zone;
            cards.Remove(card);
            selectedCard = null;

            Debug.Log($"Played Card{card.cardName}");
            
            // ͨInformm the GameManager to record the played card
            GameManager.Instance.GetCurrentPlayer().RecordDiscard(card);
            if (handUI != null)
            {
                handUI.UpdateHandUI(cards); // Update HandUI
            }
            if (controller != null)
            {
                controller.Play();
            }
            

            
        }
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards); // Returns a copy to prevent external modification
    }

    public void ClearHand()
    {
        cards.Clear();
        selectedCard = null;
    }

    public int GetCardValue()
    {
        return cards.Count > 0 ? cards[0].value : 0;
    }

    public bool CanPlayCard(Card card)
    {
        if (cards.Count == 2 && cards.Exists(c => c.value == 7))
        {
            if (card.value != 7 && card.value > 4)
            {
                Debug.Log("You can't play this card because you have a 7 and the other card is greater than 4. You must play the 7!");
                return false;
            }
        }
        return true;
    }

    public void Discard(Card card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            Debug.Log($"Discarded Card: {card.cardName}");
        }
    }
}
