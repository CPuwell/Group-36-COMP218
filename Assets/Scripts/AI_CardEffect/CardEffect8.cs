using UnityEngine;

public class CardEffect8 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} �����8���ƣ��Ա����֣�");
        currentPlayer.Eliminate(); // ������������̭�߼�
        GameManager.Instance.EndTurn();
    }
}
