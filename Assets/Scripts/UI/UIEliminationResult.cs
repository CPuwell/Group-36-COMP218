using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIEliminationResult : MonoBehaviour
{
    [Header("UI Bindings")]
    public GameObject panel;                  // Panel background (entire panel)
    public TextMeshProUGUI messageText;        // Text to display message
    public Button closeButton;                 // Close button

    /// <summary>
    /// Show the result message
    /// </summary>
    /// <param name="message">The message text to display</param>
    public void Show(string message)
    {
        panel.SetActive(true);
        messageText.text = message;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }
}
