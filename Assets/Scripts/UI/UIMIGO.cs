using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMIGO : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI messageText;
    public Image cardImage;
    public Button closeButton;

    [Header("Card No.0 Image")]
    public Sprite miGoCardSprite;

    private System.Action onClosedCallback;

    public void Show(Player targetPlayer, System.Action onClosed = null)
    {
        gameObject.SetActive(true);
        onClosedCallback = onClosed;

        messageText.text = "The targeted player is forced to add the Card Mi-Go Braincase to their hand.";
        cardImage.sprite = miGoCardSprite;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onClosedCallback?.Invoke();
        });
    }
}
