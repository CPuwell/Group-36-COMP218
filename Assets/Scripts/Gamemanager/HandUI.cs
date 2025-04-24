using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    public List<CardUI> cardSlots; // 在 Inspector 中拖入 CardSlot1 和 CardSlot2
    private List<Card> currentHand; // 当前手牌

    public Sprite backSprite; // 翻面时用的背面图
    public GameManager gameManager; // 引用你的逻辑类
    
    void Start()
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            int index = i; // 捕获当前索引
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
                // 先设置 cardObject 的父物体为 CardSlot
                var cardObj = hand[i].cardObject;
                cardObj.transform.SetParent(cardSlots[i].transform);
                cardObj.transform.localPosition = Vector3.zero;
                cardObj.transform.localScale = Vector3.one;

                // 然后设置 CardUI 数据
                var cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(hand[i], gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true); // 展示正面
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
