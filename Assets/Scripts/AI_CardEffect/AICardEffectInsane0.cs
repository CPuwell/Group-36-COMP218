using UnityEngine;

public class AICardEffectInsane0 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] You played Card 0 (Mi-Go Brain Cylinder) and are immediately eliminated.");
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] You played Card 0 (Mi-Go Brain Cylinder) and are immediately eliminated.");
        GameManager.Instance.EndTurn();
    }
}
