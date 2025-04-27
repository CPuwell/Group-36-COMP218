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
        Debug.Log($"更新手牌 UI，当前手牌数量：{hand.Count}");
        Debug.Log($"CardSlot数量{cardSlots.Count}");
        for (int i = 0; i < cardSlots.Count; i++)
        {
            // 1. 清除所有旧物体
            foreach (Transform child in cardSlots[i].transform)
            {
                Destroy(child.gameObject);
            }

            // 2. 给每个 cardSlots[i] 填充新卡牌
            if (i < hand.Count)
            {
                Card card = hand[i];
                if (card.cardObject == null)
                {
                    Debug.LogError($"手牌 {card.cardName} 的 cardObject 是 null！");
                    continue;
                }

                // ---  重点1：重新克隆一张新的 cardObject ---
                GameObject newCardObj = GameObject.Instantiate(card.cardObject);
                newCardObj.transform.SetParent(cardSlots[i].transform, false);
                newCardObj.transform.localPosition = Vector3.zero;
                newCardObj.transform.localScale = Vector3.one;
                newCardObj.transform.localRotation = Quaternion.identity;
                newCardObj.SetActive(true);

                // ---  重点2：确保 CardUI 重新绑定 ---
                CardUI cardUI = newCardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(card, gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true); // 正面朝上
                }
                else
                {
                    Debug.LogError($"新 cardObject 缺少 CardUI 组件！");
                }
            }
        }
    }




    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }

}
