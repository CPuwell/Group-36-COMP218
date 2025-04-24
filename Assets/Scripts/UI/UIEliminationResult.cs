using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIEliminationResult : MonoBehaviour
{
    [Header("UI 绑定")]
    public GameObject panel;                  // 面板背景（整体）
    public TextMeshProUGUI messageText;       // 显示内容
    public Button closeButton;                // 关闭按钮

    /// <summary>
    /// 显示提示内容
    /// </summary>
    /// <param name="message">要显示的提示文本</param>
    public void Show(string message)
    {
        panel.SetActive(true);
        messageText.text = message;

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }
}
