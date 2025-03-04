using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // 开始游戏
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");  // 加载游戏场景
    }

    // 加载存档
    public void LoadGame()
    {
        Debug.Log("加载存档功能");
        // 这里可以加入读取存档逻辑
    }

    // 进入设置界面
    public void OpenSettings()
    {
        Debug.Log("打开设置菜单");
        // 这里可以切换到设置界面
    }

    // 退出游戏
    public void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
