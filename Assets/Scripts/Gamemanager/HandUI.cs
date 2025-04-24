using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    public List<CardUI> cardSlots; // �� Inspector ������ CardSlot1 �� CardSlot2
    private List<Card> currentHand; // ��ǰ����

    public Sprite backSprite; // ����ʱ�õı���ͼ
    public GameManager gameManager; // ��������߼���
    
    void Start()
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            int index = i; // ����ǰ����
            Button btn = cardSlots[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => PlayCard(index));
            }
            
        }
    }

    public void UpdateHandUI(List<Card> hand)
    {
        currentHand = hand;

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < hand.Count)
            {
                // ������ cardObject �ĸ�����Ϊ CardSlot
                var cardObj = hand[i].cardObject;
                cardObj.transform.SetParent(cardSlots[i].transform);
                cardObj.transform.localPosition = Vector3.zero;
                cardObj.transform.localScale = Vector3.one;

                // Ȼ������ CardUI ����
                var cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(hand[i], gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true); // չʾ����
                }
                cardObj.SetActive(true);
            }
            else
            {
                cardSlots[i].gameObject.SetActive(false);
            }
        }

    }



    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }

}
