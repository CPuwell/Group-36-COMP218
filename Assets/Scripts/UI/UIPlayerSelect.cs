using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerSelect : MonoBehaviour
{
    [Header("Player Buttons")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button buttonSelf; // "Yourself" button

    [Header("Control Button")]
    public Button confirmButton;

    private List<Player> currentTargets;
    private Action<Player> onPlayerSelected;
    private Player selectedPlayer;

    public void Show(List<Player> targets, Action<Player> callback, bool includeSelf = false)
    {
        currentTargets = targets;
        onPlayerSelected = callback;
        selectedPlayer = null;

        gameObject.SetActive(true);

        // Activate/Deactivate the "Yourself" button
        buttonSelf.gameObject.SetActive(includeSelf);

        // Set click events for each button
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

        buttonSelf.onClick.RemoveAllListeners();
        buttonSelf.onClick.AddListener(() => SelectPlayerByIndex(0)); // Index 0 represents yourself

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirm);
    }

    private void SelectPlayerByIndex(int index)
    {
        selectedPlayer = currentTargets.Find(p => p.PlayerIndex == index);
        if (selectedPlayer != null)
            UIManager.Instance.Log($"Selected player {selectedPlayer.playerName}");
    }

    private void OnConfirm()
    {
        if (selectedPlayer == null)
        {
            UIManager.Instance.ShowPopup("Please select a player first!");
            return;
        }

        onPlayerSelected?.Invoke(selectedPlayer);
        gameObject.SetActive(false);
    }
}
