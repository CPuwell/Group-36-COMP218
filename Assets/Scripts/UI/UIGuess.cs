using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGuess : MonoBehaviour
{
    [Header("Fixed Player Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    [Header("Input Field")]
    public TMP_InputField inputField;

    [Header("Control Buttons")]
    public Button confirmButton;
    public Button cancelButton;

    private List<Player> currentTargets;
    private Action<Player, int> onGuessConfirmed;
    private Player selectedPlayer;

    public void Show(List<Player> targets, Action<Player, int> callback)
    {
        currentTargets = targets;
        onGuessConfirmed = callback;
        selectedPlayer = null;

        gameObject.SetActive(true);

        // Clear selection
        ResetButtons();

        // Add click event listeners to each button (index starting from 1)
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => SelectPlayerByIndex(1));

        button2.onClick.RemoveAllListeners();
        button2.onClick.AddListener(() => SelectPlayerByIndex(2));

        button3.onClick.RemoveAllListeners();
        button3.onClick.AddListener(() => SelectPlayerByIndex(3));

        button4.onClick.RemoveAllListeners();
        button4.onClick.AddListener(() => SelectPlayerByIndex(4));

        button5.onClick.RemoveAllListeners();
        button5.onClick.AddListener(() => SelectPlayerByIndex(5));

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirm);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void SelectPlayerByIndex(int index)
    {
        selectedPlayer = currentTargets.Find(p => p.PlayerIndex == index);
        UIManager.Instance.Log($"Selected player {index}");
    }

    private void OnConfirm()
    {
        if (selectedPlayer == null)
        {
            UIManager.Instance.ShowPopup("Please select a player first!");
            return;
        }

        if (!int.TryParse(inputField.text, out int guess))
        {
            UIManager.Instance.ShowPopup("Please enter a valid number (2 ~ 8)!");
            return;
        }

        if (guess < 2 || guess > 8)
        {
            UIManager.Instance.ShowPopup("You can only guess numbers between 2 and 8!");
            return;
        }

        onGuessConfirmed?.Invoke(selectedPlayer, guess);
        gameObject.SetActive(false);
    }

    private void ResetButtons()
    {
        // Optional: Clear button colors or other UI states
    }
}
