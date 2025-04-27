using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// �����ʼ��ť��������Ϸ����
    /// </summary>
    public void StartGame()
    {
        // ���� Table ������LoadSceneMode.Single ��ʾ���滻��ǰ������
        SceneManager.LoadScene("Table", LoadSceneMode.Single);
    }

    /// <summary>
    /// ����˳���ť���˳���Ϸ
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("�˳���Ϸ");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �ڱ༭����ֹͣ����
#else
        Application.Quit(); // ����������˳���Ϸ
#endif
    }
}
