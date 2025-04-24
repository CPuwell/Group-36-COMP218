using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGuess : MonoBehaviour
{
    [Header("固定玩家按钮")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    [Header("输入字段")]
    public TMP_InputField inputField;

    [Header("控制按钮")]
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

        // 清空选择
        ResetButtons();

        // 为每个按钮添加点击事件（编号从 1 开始）
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
        UIManager.Instance.Log($"已选择玩家 {index}");
    }

    private void OnConfirm()
    {
        if (selectedPlayer == null)
        {
            UIManager.Instance.ShowPopup("请选择一名玩家！");
            return;
        }

        if (!int.TryParse(inputField.text, out int guess))
        {
            UIManager.Instance.ShowPopup("请输入有效的数字（2 ~ 8）！");
            return;
        }

        if (guess < 2 || guess > 8)
        {
            UIManager.Instance.ShowPopup("你只能猜 2 到 8！");
            return;
        }

        onGuessConfirmed?.Invoke(selectedPlayer, guess);
        gameObject.SetActive(false);
    }

    private void ResetButtons()
    {
        // 可选：清除按钮颜色或其他UI状态
    }
}
