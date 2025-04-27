using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Discard Selector UI (弃牌选择)")]
    public UIDiscardSelector discardSelectorPanel;

    [Header("General Popup (通用弹窗)")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button popupCloseButton;

    [Header("Guess Card UI (猜牌1号效果)")]
    public UIGuess guessPanel;

    [Header("Player Selection UI (2、3、6号牌用)")]
    public UIPlayerSelect playerSelectPanel;

    [Header("Card Reveal UI (查看手牌)")]
    public UICardReveal cardRevealPanel;

    [Header("Mi-Go Brain Reveal UI (疯狂5号用)")]
    public UIMIGO miGoPanel;

    [Header("Victory/Defeat/Draw Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject drawPanel;
    public TextMeshProUGUI defeatReasonText; // 仅失败面板需要显示原因

    [Header("Log Output (可选)")]
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
        SceneManager.LoadScene("Table"); // 重启游戏场景
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu"); // 返回主菜单
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
}
