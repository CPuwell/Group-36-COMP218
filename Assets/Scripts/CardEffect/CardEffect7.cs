using UnityEngine;

public class CardEffect7 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} �����7���ƣ�û���κ�Ч��");
        GameManager.Instance.EndTurn();
    }
}
