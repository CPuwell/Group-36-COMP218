using UnityEngine;

public class AICardEffectInsane8 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} played Card 8 and self-eliminated!");
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] Check how many insane cards have been discarded...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} has discarded {insaneDiscards} insane cards and triggers instant victory!");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} are currently immortal. Although you did not discard enough insane cards, you survive this round.");
                GameManager.Instance.EndTurn();
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} did not discard enough insane cards. You are eliminated.");
                currentPlayer.Eliminate();
            }
        }
    }
}
