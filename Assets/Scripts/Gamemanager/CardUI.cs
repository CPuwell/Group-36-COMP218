using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public Image frontImage;
    public Image backImage;

    private Card cardData;              // 当前绑定的卡牌数据
    private bool isFaceUp = false;
    private Hand owningHand;           // 当前手牌（逻辑层）引用

    // 设置卡牌图像与数据
    public void SetCard(Card card, Hand hand)
    {
        cardData = card;
        owningHand = hand;

        frontImage.sprite = card.frontSprite;
        backImage.sprite = card.backSprite;

        Flip(false); // 初始是盖着的
    }

    // 翻牌：true 显示正面，false 显示背面
    public void Flip(bool faceUp)
    {
        isFaceUp = faceUp;
        frontImage.gameObject.SetActive(faceUp);
        backImage.gameObject.SetActive(!faceUp);
    }

    // 点击事件处理（双击出牌）
    public void OnPointerClick(PointerEventData eventData)
    {
        owningHand.SelectCard(cardData);
    }

    //elegate 事件：当卡牌被选中时调用
    public void SetSelected(bool selected)
    {
        GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }

}
