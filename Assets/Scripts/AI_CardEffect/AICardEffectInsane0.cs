using UnityEngine;

public class AICardEffectInsane0 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        UIManager.Instance.ShowPopup($" {currentPlayer.playerName} played Card 0 (Mi-Go Brain Cylinder) and are immediately eliminated.");
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        UIManager.Instance.ShowPopup($" {currentPlayer.playerName} played Card 0 (Mi-Go Brain Cylinder) and are immediately eliminated.");
        GameManager.Instance.EndTurn();
    }
}
