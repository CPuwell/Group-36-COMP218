using UnityEngine;

public class AICardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] You are protected and will go insane after this.");

        currentPlayer.SetProtected(true);
        currentPlayer.GoInsane();

        UIManager.Instance.UpdateImmortalIndicators(GameManager.Instance.players);

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} cannot be chosen as part of the effects of other players' cards until the start of their next turn.");

        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] You become immortal until the end of this round.");

        currentPlayer.SetImmortalThisRound(true);

        UIManager.Instance.UpdateImmortalIndicators(GameManager.Instance.players);

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} will not be eliminated until the end of this round.");

        GameManager.Instance.EndTurn();
    }
}
