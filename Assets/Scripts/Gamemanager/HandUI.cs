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
            // 清除旧的卡牌 GameObject
            foreach (Transform child in cardSlots[i].transform)
            {
                // 不再销毁 GameObject，只隐藏它
                child.gameObject.SetActive(false);
            }


            if (i < hand.Count)
            {
                Card card = hand[i];
                GameObject cardObj = card.cardObject;

                if (cardObj == null)
                {
                    Debug.LogError($"手牌 {card.cardName} 的 cardObject 为 null！");
                    continue;
                }

                // 设置 parent 为对应的 cardSlot
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
                    rt.SetAsLastSibling(); // 确保显示在最上层
                }

                cardObj.SetActive(true);



                // 重新绑定 CardUI 信息
                CardUI cardUI = cardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(card, gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true);
                    Debug.Log($"SetCard成功");
                }
                else
                {
                    Debug.LogError("cardObject 上找不到 CardUI 脚本！");
                }
            }
        }
    }




    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }

}
