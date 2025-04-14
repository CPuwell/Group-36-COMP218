using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    public List<Image> cardSlots; // 在 Inspector 中拖入 CardSlot1 和 CardSlot2
    private List<Card> currentHand; // 当前手牌

    public Sprite backSprite; // 翻面时用的背面图
    public GameManager gameManager; // 引用你的逻辑类

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
