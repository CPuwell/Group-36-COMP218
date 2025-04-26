using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Hand hand = new Hand();
    public HandUI handUI;
    
    public void AddCard(Card card)
    {
        hand.AddCard(card);
        // ������Բ���Ҫ�ֶ� UpdateHandUI �ˣ������¼�ϵͳ�Զ���
    }

    public void OnCardClicked(Card card)
    {
        hand.SelectCard(card);
        // ����Ҳ����ǿ��UpdateHandUI�ˣ�����ƺ�Hand�Լ��ᴥ��OnHandChanged
    }
}
