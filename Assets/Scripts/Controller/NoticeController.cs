using UnityEngine;
using TMPro;

public class NoticeController : MonoBehaviour
{
    public static NoticeController Instance;

    public TextMeshProUGUI noticeText;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// �����ı�����
    /// </summary>
    public void SetNotice(string message)
    {
        if (noticeText != null)
        {
            noticeText.text = message;
        }
    }
}
