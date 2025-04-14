using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Hand hand = new Hand();
    public HandUI handUI;

    public void AddCard(Card card)
    {
        hand.AddCard(card);
        handUI.UpdateHandUI(hand.GetCards());
    }

    public void OnCardClicked(Card card)
    {
        hand.SelectCard(card);
        handUI.UpdateHandUI(hand.GetCards());
    }
}
