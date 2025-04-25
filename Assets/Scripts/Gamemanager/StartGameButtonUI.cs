using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    public GameManager gameManager;
    public Player humanPlayer; // 在 Inspector 中手动拖入 Player0
    public Button startButton;

    public void StartGame()
    {
        if (gameManager == null || humanPlayer == null)
        {
            Debug.LogWarning("GameManager 或 HumanPlayer 没有绑定！");
            return;
        }

        //  添加人类玩家（场景中已有对象）
        humanPlayer.isHuman = true;
        gameManager.players.Add(humanPlayer);

        //  添加 5 个 AI 玩家
        for (int i = 2; i <= 6; i++)
        {
            gameManager.AddPlayer($"Bot {i}");
        }

        //  启动游戏
        gameManager.StartGame();

        //  隐藏按钮
        startButton.gameObject.SetActive(false);
    }

}
