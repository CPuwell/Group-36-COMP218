using UnityEngine;

public class CardEffect4 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        currentPlayer.SetProtected(true);
        Debug.Log($"{currentPlayer.playerName} �����ܵ�������ֱ�����´λغϿ�ʼ");
        GameManager.Instance.EndTurn();
    }
}
