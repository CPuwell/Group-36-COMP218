using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerSelect : MonoBehaviour
{
    [Header("玩家按钮")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button buttonSelf; // "yourself" 按钮

    [Header("控制按钮")]
    public Button confirmButton;
    public Button cancelButton;

    private List<Player> currentTargets;
    private Action<Player> onPlayerSelected;
    private Player selectedPlayer;

    public void Show(List<Player> targets, Action<Player> callback, bool includeSelf = false)
    {
        currentTargets = targets;
        onPlayerSelected = callback;
        selectedPlayer = null;

        gameObject.SetActive(true);

        // 激活/隐藏 “yourself” 按钮
        buttonSelf.gameObject.SetActive(includeSelf);

        // 设置每个按钮的点击事件
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
        buttonSelf.onClick.AddListener(() => SelectPlayerByIndex(0)); // 0号是自己

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
        if (selectedPlayer != null)
            UIManager.Instance.Log($"已选择玩家 {selectedPlayer.playerName}");
    }

    private void OnConfirm()
    {
        if (selectedPlayer == null)
        {
            UIManager.Instance.ShowPopup("请选择一名玩家！");
            return;
        }

        onPlayerSelected?.Invoke(selectedPlayer);
        gameObject.SetActive(false);
    }
}
