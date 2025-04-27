using UnityEngine;

using UnityEngine.SceneManagement;

using System.Collections;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// 点击开始按钮，加载游戏场景
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
    /// 点击退出按钮，退出游戏
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("退出游戏");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器里停止播放
#else
        Application.Quit(); // 打包后正常退出游戏
#endif
    }
}
