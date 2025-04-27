using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDiscardSelector discardSelectorPanel;

    [Header("通用弹窗")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button popupCloseButton;
    [Header("猜牌 UI（用于1号牌）")]
    public UIGuess guessPanel;

    [Header("玩家选择 UI（用于2、3、6号牌）")]
    public UIPlayerSelect playerSelectPanel;

    [Header("展示卡牌 UI（用于展示目标手牌）")]
    public UICardReveal cardRevealPanel;

    [Header("Mi-Go 0号牌 UI")]
    public UIMIGO miGoPanel;

    [Header("日志输出（可选）")]
    public TextMeshProUGUI logTextArea;
 


    private void Awake()
    {
        Instance = this;
    }

    // ========== 通用弹窗 ==========
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

    // ========== 猜牌（用于1号牌）==========
    public void ShowGuessEffect(List<Player> targets, Action<Player, int> onGuessConfirmed)
    {
        if (targets == null || targets.Count == 0)
        {
            ShowPopup("没有其他玩家可供猜牌");
            return;
        }

        guessPanel.Show(targets, onGuessConfirmed);
    }

    // ========== 简单玩家选择（用于2、3、6号牌）==========
    public void ShowPlayerSelectionSimple(List<Player> players, Action<Player> onSelected)
    {
        if (players == null || players.Count == 0)
        {
            ShowPopup("没有可供选择的玩家");
            return;
        }

        playerSelectPanel.Show(players, onSelected);
    }

    // ========== 展示卡牌（用于2号牌 ShowCard）==========
    public void ShowCardReveal(Card card, string ownerName, Action onClosed = null)
    {
        cardRevealPanel.Show(card, ownerName, onClosed);
    }

    // ========== 日志输出 ==========
    public void Log(string message)
    {
        Debug.Log("[UI] " + message);

        if (logTextArea != null)
        {
            logTextArea.text += message + "\n";
        }
    }

    // ========== 通用关闭 ==========
    public void HideAllPanels()
    {
        popupPanel?.SetActive(false);
        guessPanel?.gameObject.SetActive(false);
        playerSelectPanel?.gameObject.SetActive(false);
        cardRevealPanel?.gameObject.SetActive(false);
    }
    
 
    // 5号牌
    public void ShowPlayerSelectionAllowSelf(List<Player> players, Action<Player> onSelected)
    {
        playerSelectPanel.Show(players, onSelected, true);
    }

    public void ShowDiscardSelector(Card card1, Card card2, Action<Card> onChosen)
    {
        discardSelectorPanel.Show(card1, card2, onChosen);
    }

    public void ShowMiGoBrainReveal(Player target, System.Action onClosed = null)
    {
        if (miGoPanel != null)
        {
            miGoPanel.Show(target, onClosed);
        }
    }


}
