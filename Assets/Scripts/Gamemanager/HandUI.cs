using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    public List<Image> cardSlots; // �� Inspector ������ CardSlot1 �� CardSlot2
    private List<Card> currentHand; // ��ǰ����

    public Sprite backSprite; // ����ʱ�õı���ͼ
    public GameManager gameManager; // ��������߼���

    public void UpdateHandUI(List<Card> hand)
    {
        currentHand = hand;

        for (int i = 0; i < cardSlots.Count; i++)
        {
            if (i < hand.Count)
            {
                cardSlots[i].sprite = hand[i].frontSprite;
                cardSlots[i].GetComponent<Button>().interactable = true;
                int index = i;
                cardSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                cardSlots[i].GetComponent<Button>().onClick.AddListener(() => PlayCard(index));
            }
            else
            {
                cardSlots[i].sprite = null;
                cardSlots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

}
