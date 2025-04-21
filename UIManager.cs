using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//通用弹窗
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


// 1:弹出选择玩家 + 猜数字（理智/疯狂）组合界面
public void ShowGuessEffect(List<Player> targets, Action<Player, int> onGuessConfirmed)
{
    // 第一步：选择玩家
    ShowPlayerSelection(targets, selectedPlayer =>
    {
        // 第二步：选择数字（排除1号，理智状态不能选1；疯狂第二次可以选）
        List<int> guessableNumbers = new List<int>();
        for (int i = 2; i <= 8; i++) guessableNumbers.Add(i);

        ShowNumberSelection(guessableNumbers, guessedNumber =>
        {
            onGuessConfirmed?.Invoke(selectedPlayer, guessedNumber);
        });
    });
}

// 2: 查看其他玩家的手牌
public void ShowCardReveal(List<Player> targets, System.Action<Player> onRevealed)
{
    ShowPlayerSelection(targets, selectedPlayer =>
    {
        // 获取手牌
        List<Card> cards = selectedPlayer.GetCards();

        if (cards.Count > 0)
        {
            string cardName = cards[0].cardName;
            ShowPopup($"{selectedPlayer.playerName} 的手牌是：{cardName}");
        }
        else
        {
            ShowPopup($"{selectedPlayer.playerName} 没有手牌");
        }

        onRevealed?.Invoke(selectedPlayer);
    });
}

// 3:选择一个目标玩家，回调中进行手牌比较处理
public void ShowDuelTargetSelection(List<Player> targets, System.Action<Player> onTargetSelected)
{
    ShowPlayerSelection(targets, onTargetSelected);
}

// 4:金身(未加入cardeffect)
//  显示玩家获得保护状态的提示
// public void ShowProtectionNotice(Player player)
// {
//     ShowPopup($"{player.playerName} 现在受到保护，直到他下次回合开始为止。\n任何针对他的卡牌效果将无效。");
// }

// 5: 指定一名玩家，该玩家弃掉手牌并从牌堆顶抽一张新手牌（可选自己）。
// 通过 ShowPlayerSelection 实现。


// 6: 显示玩家选择弹窗（用于交换手牌）
public void ShowCardSwapSelection(List<Player> targets, System.Action<Player> onTargetSelected)
{
    ShowPlayerSelection(targets, onTargetSelected);
}
