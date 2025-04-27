using UnityEngine;

public class AICardEffectInsane8 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] No immediate effect. Only affects end-of-round judgment for insane players.");
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
            UIManager.Instance.ShowPopup("You have discarded at least 2 insane cards! You immediately win the game!");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                UIManager.Instance.ShowPopup("You are currently immortal. Although you did not discard enough insane cards, you survive this round.");
                GameManager.Instance.EndTurn();
            }
            else
            {
                UIManager.Instance.ShowPopup("You did not discard enough insane cards. You are eliminated.");
                currentPlayer.Eliminate();
            }
        }
    }
}
