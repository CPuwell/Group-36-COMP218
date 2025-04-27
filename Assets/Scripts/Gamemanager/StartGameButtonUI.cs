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

        humanPlayer.Initialize(gameManager.GetNextPlayerIndex(), "Player");

        gameManager.players.Add(humanPlayer);

        //  ��� 5 �� AI ���
        for (int i = 1; i <= 5; i++)
        {
            gameManager.AddPlayer($"Bot {i}");
        }

        //  ������Ϸ
        gameManager.StartGame();

        //  ���ذ�ť
        startButton.gameObject.SetActive(false);
    }

}
