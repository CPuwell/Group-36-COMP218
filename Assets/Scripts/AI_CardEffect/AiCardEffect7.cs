using UnityEngine;

public class AiCardEffect7 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} 打出了7号牌，没有任何效果");
        GameManager.Instance.EndTurn();
    }
}
