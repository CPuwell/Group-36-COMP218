using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDiscardSelector : MonoBehaviour
{
    [Header("Card Display")]
    public Image cardImageLeft;
    public Image cardImageRight;

    [Header("Selection Buttons")]
    public Button chooseLeftButton;
    public Button chooseRightButton;

    [Header("Title Text (optional)")]
    public TextMeshProUGUI titleText;

    private Card leftCard;
    private Card rightCard;
    private Action<Card> onCardSelected;

    /// <summary>
    /// Show the discard selection panel
    /// </summary>
    /// <param name="card1">Left card</param>
    /// <param name="card2">Right card</param>
    /// <param name="onChosen">Callback triggered after discarding</param>
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
            titleText.text = "Please discard one of the cards below";
        }
    }

    private void SelectCard(Card chosen)
    {
        Debug.Log($"Player selected to discard: {chosen.cardName}");
        onCardSelected?.Invoke(chosen);
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
