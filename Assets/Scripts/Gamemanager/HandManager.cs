using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Hand hand = new Hand();
    public HandUI handUI;
    
    public void AddCard(Card card)
    {
        hand.AddCard(card);
        // 这里可以不需要手动 UpdateHandUI 了，交给事件系统自动做
    }

    public void OnCardClicked(Card card)
    {
        hand.SelectCard(card);
        // 这里也不用强行UpdateHandUI了，打出牌后Hand自己会触发OnHandChanged
    }
}
