using UnityEngine;

public class AiCardEffect7 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} played Card 7. No effect.");
        GameManager.Instance.EndTurn();
    }
}
