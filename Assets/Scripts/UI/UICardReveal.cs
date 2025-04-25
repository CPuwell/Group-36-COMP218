using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICardReveal : MonoBehaviour
{
    [Header("UI ×é¼þ")]
    public Image cardImage;
    public TextMeshProUGUI titleText;
    public Button closeButton;

    private Action onClosed;

    public void Show(Card card, string ownerName, Action callback = null)
    {
        gameObject.SetActive(true);

        titleText.text = $"Show card of {ownerName}";
        cardImage.sprite = card.frontSprite;
        onClosed = callback;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onClosed?.Invoke();
        });
    }
}
