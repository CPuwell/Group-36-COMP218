using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    public GameManager gameManager;
    public Player humanPlayer; // �� Inspector ���ֶ����� Player0
    public Button startButton;

    public void StartGame()
    {
        if (gameManager == null || humanPlayer == null)
        {
            Debug.LogWarning("GameManager �� HumanPlayer û�а󶨣�");
            return;
        }

        //  ���������ң����������ж���
        humanPlayer.isHuman = true;
        gameManager.players.Add(humanPlayer);

        //  ��� 5 �� AI ���
        for (int i = 2; i <= 6; i++)
        {
            gameManager.AddPlayer($"Bot {i}");
        }

        //  ������Ϸ
        gameManager.StartGame();

        //  ���ذ�ť
        startButton.gameObject.SetActive(false);
    }

}
