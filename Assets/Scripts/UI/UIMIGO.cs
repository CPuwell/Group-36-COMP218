using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMIGO : MonoBehaviour
{
    [Header("UI ÔªËØ")]
    public TextMeshProUGUI messageText;
    public Image cardImage;
    public Button closeButton;

    [Header("0ºÅÅÆÍ¼Æ¬")]
    public Sprite miGoCardSprite;

    private System.Action onClosedCallback;

    public void Show(Player targetPlayer, System.Action onClosed = null)
    {
        gameObject.SetActive(true);
        onClosedCallback = onClosed;

        messageText.text = $"The targeted player is forced to add the Card Mi-Go Braincase to their hand.";
        cardImage.sprite = miGoCardSprite;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onClosedCallback?.Invoke();
        });
    }
}
