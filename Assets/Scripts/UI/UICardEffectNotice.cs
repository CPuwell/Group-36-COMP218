using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UICardEffectNotice : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI descriptionText;
    public Button confirmButton;
    public Button cancelButton;

    private Action onConfirmed;

    public void Show(string message, Action onConfirmAction)
    {
        panel.SetActive(true);
        descriptionText.text = message;
        onConfirmed = onConfirmAction;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            onConfirmed?.Invoke();
            panel.SetActive(false);
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }
}
