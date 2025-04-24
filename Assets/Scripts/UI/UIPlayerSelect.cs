using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerSelect : MonoBehaviour
{
    [Header("��Ұ�ť")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button buttonSelf; // "yourself" ��ť

    [Header("���ư�ť")]
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

        // ����/���� ��yourself�� ��ť
        buttonSelf.gameObject.SetActive(includeSelf);

        // ����ÿ����ť�ĵ���¼�
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
        buttonSelf.onClick.AddListener(() => SelectPlayerByIndex(0)); // 0�����Լ�

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
            UIManager.Instance.Log($"��ѡ����� {selectedPlayer.playerName}");
    }

    private void OnConfirm()
    {
        if (selectedPlayer == null)
        {
            UIManager.Instance.ShowPopup("��ѡ��һ����ң�");
            return;
        }

        onPlayerSelected?.Invoke(selectedPlayer);
        gameObject.SetActive(false);
    }
}
