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
            // ����ɵĿ��� GameObject
            foreach (Transform child in cardSlots[i].transform)
            {
                // �������� GameObject��ֻ������
                child.gameObject.SetActive(false);
            }


            if (i < hand.Count)
            {
                Card card = hand[i];
                GameObject cardObj = card.cardObject;

                if (cardObj == null)
                {
                    Debug.LogError($"���� {card.cardName} �� cardObject Ϊ null��");
                    continue;
                }

                // ���� parent Ϊ��Ӧ�� cardSlot
                cardObj.transform.SetParent(cardSlots[i].transform, false);



                RectTransform rt = cardObj.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = Vector3.one;
                    rt.localRotation = Quaternion.identity;
                    rt.SetAsLastSibling(); // ȷ����ʾ�����ϲ�
                }

                cardObj.SetActive(true);



                // ���°� CardUI ��Ϣ
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(card, gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true);
                    Debug.Log($"SetCard�ɹ�");
                }
                else
                {
                    Debug.LogError("cardObject ���Ҳ��� CardUI �ű���");
                }
            }
        }
    }




    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }

}
