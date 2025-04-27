using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Hand hand = new Hand();
    public HandUI handUI;
    
    public void AddCard(Card card)
    {
        hand.AddCard(card);
        // There is no need to manually UpdateHandUI here, let the event system do it automatically
    }

    public void OnCardClicked(Card card)
    {
        hand.SelectCard(card);
        // There is no need to force UpdateHandUI here. After playing the card, Hand will trigger OnHandChanged by itself.
    }
}
