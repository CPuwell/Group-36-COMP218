using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Discard Selector UI")]
    public UIDiscardSelector discardSelectorPanel;

    [Header("General Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button popupCloseButton;

    [Header("Guess Card UI")]
    public UIGuess guessPanel;

    [Header("Player Selection UI")]
    public UIPlayerSelect playerSelectPanel;

    [Header("Card Reveal UI")]
    public UICardReveal cardRevealPanel;

    [Header("Mi-Go Brain Reveal UI")]
    public UIMIGO miGoPanel;

    [Header("Victory/Defeat/Draw Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject drawPanel;
    public TextMeshProUGUI defeatReasonText; 

    [Header("Log Output")]
    public TextMeshProUGUI logTextArea;

    [Header("AI Turn Indicators")]
    public List<GameObject> aiTurnIndicators;

    [Header("AI Immortal/Protected Indicators")]
    public List<GameObject> aiImmortalIndicators = new List<GameObject>();

    [Header("Bot Elimination Overlays (Red X)")]
    public List<GameObject> eliminatedOverlays;


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

    // ========= Guess Card =========
    public void ShowGuessEffect(List<Player> targets, Action<Player, int> onGuessConfirmed)
    {
        if (targets == null || targets.Count == 0)
        {
            ShowPopup("No other players available for guessing");
            return;
        }

        guessPanel.Show(targets, onGuessConfirmed);
    }

    // ========= Player Selection =========
    public void ShowPlayerSelectionSimple(List<Player> players, Action<Player> onSelected)
    {
        if (players == null || players.Count == 0)
        {
            ShowPopup("No players available for selection");
            return;
        }

        playerSelectPanel.Show(players, onSelected);
    }

    public void ShowPlayerSelectionAllowSelf(List<Player> players, Action<Player> onSelected)
    {
        if (players == null || players.Count == 0)
        {
            ShowPopup("No players available for selection");
            return;
        }

        playerSelectPanel.Show(players, onSelected, true);
    }

    // ========= Card Reveal =========
    public void ShowCardReveal(Card card, string ownerName, Action onClosed = null)
    {
        cardRevealPanel.Show(card, ownerName, onClosed);
    }

    // ========= Discard Selector =========
    public void ShowDiscardSelector(Card card1, Card card2, Action<Card> onChosen)
    {
        discardSelectorPanel.Show(card1, card2, onChosen);
    }

    // ========= Mi-Go Reveal =========
    public void ShowMiGoBrainReveal(Player target, Action onClosed = null)
    {
        if (miGoPanel != null)
        {
            miGoPanel.Show(target, onClosed);
        }
    }

    // ========= Victory/Defeat/Draw Panels =========
    public void ShowVictoryPanel()
    {
        HideAllPanels();
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void ShowDefeatPanel(string reason)
    {
        HideAllPanels();
        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);

            if (defeatReasonText != null)
                defeatReasonText.text = reason;
        }
    }

    public void ShowDrawPanel()
    {
        HideAllPanels();
        if (drawPanel != null)
        {
            drawPanel.SetActive(true);
        }
    }

    // ========= Common Actions =========
    public void RestartGame()
    {
        SceneManager.LoadScene("Table"); 
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); 
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
        discardSelectorPanel?.gameObject.SetActive(false);
        miGoPanel?.gameObject.SetActive(false);
        victoryPanel?.SetActive(false);
        defeatPanel?.SetActive(false);
        drawPanel?.SetActive(false);
    }

    public void ShowAITurnIndicatorByPlayer(Player currentPlayer)
    {
        for (int i = 0; i < aiTurnIndicators.Count; i++)
        {
            if (GameManager.Instance.players[i + 1] == currentPlayer)
            {
                aiTurnIndicators[i].SetActive(true);
            }
            else
            {
                aiTurnIndicators[i].SetActive(false);
            }
        }
    }

    public void ClearAITurnIndicators()
    {
        foreach (GameObject indicator in aiTurnIndicators)
        {
            indicator.SetActive(false);
        }
    }

    public void UpdateImmortalIndicators(List<Player> players)
    {

        foreach (var indicator in aiImmortalIndicators)
        {
            indicator.SetActive(false);
        }


        foreach (Player player in players)
        {
            if (player.IsHuman()) continue; 

            if ((player.IsProtected() || player.IsImmortal()) && player.IsAlive())
            {
                int index = player.PlayerIndex - 1; 
                if (index >= 0 && index < aiImmortalIndicators.Count)
                {
                    aiImmortalIndicators[index].SetActive(true);
                }
            }
        }
    }
    public void ShowEliminationX(Player player)
    {
        if (player.IsHuman()) return; 

        int index = player.PlayerIndex - 1; 
        if (index >= 0 && index < eliminatedOverlays.Count)
        {
            eliminatedOverlays[index].SetActive(true);
        }
    }


}