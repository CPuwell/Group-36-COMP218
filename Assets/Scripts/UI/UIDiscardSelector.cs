using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDiscardSelector : MonoBehaviour
{
    [Header("����չʾ")]
    public Image cardImageLeft;
    public Image cardImageRight;

    [Header("ѡ��ť")]
    public Button chooseLeftButton;
    public Button chooseRightButton;

    [Header("��ʾ���֣���ѡ��")]
    public TextMeshProUGUI titleText;

    private Card leftCard;
    private Card rightCard;
    private Action<Card> onCardSelected;

    /// <summary>
    /// ��ʾ����ѡ�����
    /// </summary>
    /// <param name="card1">��߿�</param>
    /// <param name="card2">�ұ߿�</param>
    /// <param name="onChosen">���ƺ󴥷��Ļص�</param>
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
        Debug.Log($"���ѡ�����ƣ�{chosen.cardName}");
        onCardSelected?.Invoke(chosen);
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
