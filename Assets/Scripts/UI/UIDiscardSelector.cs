using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDiscardSelector : MonoBehaviour
{
    [Header("卡牌展示")]
    public Image cardImageLeft;
    public Image cardImageRight;

    [Header("选择按钮")]
    public Button chooseLeftButton;
    public Button chooseRightButton;

    [Header("提示文字（可选）")]
    public TextMeshProUGUI titleText;

    private Card leftCard;
    private Card rightCard;
    private Action<Card> onCardSelected;

    /// <summary>
    /// 显示弃牌选择界面
    /// </summary>
    /// <param name="card1">左边卡</param>
    /// <param name="card2">右边卡</param>
    /// <param name="onChosen">弃牌后触发的回调</param>
    public void Show(Card card1, Card card2, Action<Card> onChosen)
    {
        gameObject.SetActive(true);

        leftCard = card1;
        rightCard = card2;
        onCardSelected = onChosen;

        if (cardImageLeft != null) cardImageLeft.sprite = card1.frontSprite;
        if (cardImageRight != null) cardImageRight.sprite = card2.frontSprite;

        chooseLeftButton.onClick.RemoveAllListeners();
        chooseLeftButton.onClick.AddListener(() => SelectCard(leftCard));

        chooseRightButton.onClick.RemoveAllListeners();
        chooseRightButton.onClick.AddListener(() => SelectCard(rightCard));

        if (titleText != null)
        {
            titleText.text = "Please discard a card below";
        }
    }

    private void SelectCard(Card chosen)
    {
        Debug.Log($"玩家选择弃牌：{chosen.cardName}");
        onCardSelected?.Invoke(chosen);
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
