using UnityEngine;
using UnityEngine.UI;

public class UIInsaneChoice : MonoBehaviour
{
    public static UIInsaneChoice Instance;

    public GameObject panel;
    public Button saneButton;
    public Button insaneButton;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(System.Action onSane, System.Action onInsane)
    {
        panel.SetActive(true);

        // ����ɼ���
        saneButton.onClick.RemoveAllListeners();
        insaneButton.onClick.RemoveAllListeners();

        // �����߼�
        saneButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            onSane?.Invoke();
        });

        insaneButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            onInsane?.Invoke();
        });
    }
}
