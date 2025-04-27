using UnityEngine;

public class AiCardEffect8 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} played Card 8 and self-eliminated!");
        currentPlayer.Eliminate(); // Execute actual elimination logic
        GameManager.Instance.EndTurn();
    }
}
