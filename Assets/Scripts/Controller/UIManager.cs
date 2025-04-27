using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDiscardSelector discardSelectorPanel;

    [Header("General Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button popupCloseButton;

    [Header("Guess Card UI (for Card No.1)")]
    public UIGuess guessPanel;

    [Header("Player Selection UI (for Cards No.2, 3, 6)")]
    public UIPlayerSelect playerSelectPanel;

    [Header("Card Reveal UI (to show target's hand)")]
    public UICardReveal cardRevealPanel;

    [Header("Mi-Go Card UI (for Card No.0)")]
    public UIMIGO miGoPanel;

    [Header("Log Output (optional)")]
    public TextMeshProUGUI logTextArea;

    private void Awake()
    {
        Instance = this;
    }

    // ========= General Popup =========
    public void ShowPopup(string message)
    {
        popupPanel.SetActive(true);
        popupText.text = message;

        popupCloseButton.onClick.RemoveAllListeners();
        popupCloseButton.onClick.AddListener(() =>
        {
            popupPanel.SetActive(false);
        });
    }

    // ========= Guess Card (for Card No.1) =========
    public void ShowGuessEffect(List<Player> targets, Action<Player, int> onGuessConfirmed)
    {
        if (targets == null || targets.Count == 0)
        {
            ShowPopup("No other players available for guessing");
            return;
        }

        guessPanel.Show(targets, onGuessConfirmed);
    }

    // ========= Simple Player Selection (for Cards No.2, 3, 6) =========
    public void ShowPlayerSelectionSimple(List<Player> players, Action<Player> onSelected)
    {
        if (players == null || players.Count == 0)
        {
            ShowPopup("No players available for selection");
            return;
        }

        playerSelectPanel.Show(players, onSelected);
    }

    // ========= Card Reveal (for Card No.2 ShowCard) =========
    public void ShowCardReveal(Card card, string ownerName, Action onClosed = null)
    {
        cardRevealPanel.Show(card, ownerName, onClosed);
    }

    // ========= Log Output =========
    public void Log(string message)
    {
        Debug.Log("[UI] " + message);

        if (logTextArea != null)
        {
            logTextArea.text += message + "\n";
        }
    }

    // ========= Hide All Panels =========
    public void HideAllPanels()
    {
        popupPanel?.SetActive(false);
        guessPanel?.gameObject.SetActive(false);
        playerSelectPanel?.gameObject.SetActive(false);
        cardRevealPanel?.gameObject.SetActive(false);
    }

    // ========= Player Selection Allowing Self (for Card No.5) =========
    public void ShowPlayerSelectionAllowSelf(List<Player> players, Action<Player> onSelected)
    {
        playerSelectPanel.Show(players, onSelected, true);
    }

    // ========= Discard Selector (for Card No.4) =========
    public void ShowDiscardSelector(Card card1, Card card2, Action<Card> onChosen)
    {
        discardSelectorPanel.Show(card1, card2, onChosen);
    }

    // ========= Mi-Go Brain Reveal =========
    public void ShowMiGoBrainReveal(Player target, Action onClosed = null)
    {
        if (miGoPanel != null)
        {
            miGoPanel.Show(target, onClosed);
        }
    }
}

