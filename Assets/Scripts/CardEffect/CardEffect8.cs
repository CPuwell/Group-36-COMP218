using UnityEngine;

public class CardEffect8 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} �����8���ƣ��Ա����֣�");
        GameManager.Instance.EndTurn();
    }
}
