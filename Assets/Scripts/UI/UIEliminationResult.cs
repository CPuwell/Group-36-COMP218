using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIEliminationResult : MonoBehaviour
{
    [Header("UI ��")]
    public GameObject panel;                  // ��屳�������壩
    public TextMeshProUGUI messageText;       // ��ʾ����
    public Button closeButton;                // �رհ�ť

    /// <summary>
    /// ��ʾ��ʾ����
    /// </summary>
    /// <param name="message">Ҫ��ʾ����ʾ�ı�</param>
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
