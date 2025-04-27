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
        Debug.Log($"�������� UI����ǰ����������{hand.Count}");
        Debug.Log($"CardSlot����{cardSlots.Count}");
        for (int i = 0; i < cardSlots.Count; i++)
        {
            // 1. ������о�����
            foreach (Transform child in cardSlots[i].transform)
            {
                Destroy(child.gameObject);
            }

            // 2. ��ÿ�� cardSlots[i] ����¿���
            if (i < hand.Count)
            {
                Card card = hand[i];
                if (card.cardObject == null)
                {
                    Debug.LogError($"���� {card.cardName} �� cardObject �� null��");
                    continue;
                }

                // ---  �ص�1�����¿�¡һ���µ� cardObject ---
                GameObject newCardObj = GameObject.Instantiate(card.cardObject);
                newCardObj.transform.SetParent(cardSlots[i].transform, false);
                newCardObj.transform.localPosition = Vector3.zero;
                newCardObj.transform.localScale = Vector3.one;
                newCardObj.transform.localRotation = Quaternion.identity;
                newCardObj.SetActive(true);

                // ---  �ص�2��ȷ�� CardUI ���°� ---
                CardUI cardUI = newCardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(card, gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true); // ���泯��
                }
                else
                {
                    Debug.LogError($"�� cardObject ȱ�� CardUI �����");
                }
            }
        }
    }




    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }

}
