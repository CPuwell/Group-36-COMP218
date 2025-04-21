using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // UI 引用字段（需要你在 Inspector 里绑定）
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public Button popupCloseButton;
    public GameObject playerSelectionPanel;
    public Transform playerButtonContainer;
    public GameObject playerButtonPrefab;

    public void Awake()
    {
        Instance = this;
    }

    // 通用弹窗
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

    // 选择一个玩家
    public void ShowPlayerSelection(List<Player> players, Action<Player> onSelected)
    {
        if (players == null || players.Count == 0)
        {
            ShowPopup("没有可供选择的玩家");
            return;
        }

        playerSelectionPanel.SetActive(true);

        foreach (Transform child in playerButtonContainer)
            Destroy(child.gameObject);

        foreach (Player p in players)
        {
            GameObject btnObj = Instantiate(playerButtonPrefab, playerButtonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = p.playerName;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                onSelected?.Invoke(p);
                playerSelectionPanel.SetActive(false);
            });
        }
    }

    // 1:弹出选择玩家 + 猜数字组合界面
    public void ShowGuessEffect(List<Player> targets, Action<Player, int> onGuessConfirmed)
    {
        ShowPlayerSelection(targets, selectedPlayer =>
        {
            List<int> guessableNumbers = new List<int>();
            for (int i = 2; i <= 8; i++) guessableNumbers.Add(i);

            ShowNumberSelection(guessableNumbers, guessedNumber =>
            {
                onGuessConfirmed?.Invoke(selectedPlayer, guessedNumber);
            });
        });
    }

    // 2: 查看手牌
    public void ShowCardReveal(List<Player> targets, Action<Player> onRevealed)
    {
        ShowPlayerSelection(targets, selectedPlayer =>
        {
            List<Card> cards = selectedPlayer.GetCards();

            if (cards.Count > 0)
                ShowPopup($"{selectedPlayer.playerName} 的手牌是：{cards[0].cardName}");
            else
                ShowPopup($"{selectedPlayer.playerName} 没有手牌");

            onRevealed?.Invoke(selectedPlayer);
        });
    }

    // 3: 比较手牌
    public void ShowDuelTargetSelection(List<Player> targets, Action<Player> onTargetSelected)
    {
        ShowPlayerSelection(targets, onTargetSelected);
    }

    // 4: 交换卡牌
    public void ShowCardSwapSelection(List<Player> targets, Action<Player> onTargetSelected)
    {
        ShowPlayerSelection(targets, onTargetSelected);
    }

    // 5: 选数字（你可能已写过）
    public void ShowNumberSelection(List<int> numbers, Action<int> onNumberSelected)
    {
        // 这里你可以自己定义你用的数字选择 UI
    }

    public void Log(string message)
    {
        Debug.Log($"[UI] {message}");
    }

}
