using UnityEngine;

public class CardEffect4 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        currentPlayer.SetProtected(true);
        Debug.Log($"{currentPlayer.playerName} �����ܵ�������ֱ�����´λغϿ�ʼ");
        GameManager.Instance.EndTurn();
    }
}
