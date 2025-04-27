using UnityEngine;

using UnityEngine.SceneManagement;

using System.Collections;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// �����ʼ��ť��������Ϸ����
    /// </summary>
    public void StartGame()
    {
        
        StartCoroutine(LoadUISceneAndStartGame());
    }

    private IEnumerator LoadUISceneAndStartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Table", LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait for next frame to continue checking
        }
        Debug.Log("Game Scene Loaded Successfully!");
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
