using UnityEngine;

public class AiCardEffect4 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log("[Sanity Effect] You are protected and cannot be targeted by other players until the start of your next turn.");

        // Set the player as protected
        currentPlayer.SetProtected(true);

        UIManager.Instance.UpdateImmortalIndicators(GameManager.Instance.players);

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} cannot be chosen as part of the effects of other players' cards until the start of their next turn.");

        GameManager.Instance.EndTurn();
    }
}
