using UnityEngine;

public class AiCardEffect4 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】你获得保护，直到下次回合开始，不能被其他玩家选中");

        // 设置玩家为 protected
        currentPlayer.SetProtected(true);

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} cannot be chosen as part of the effects of other players' cards until the start of his next turn.");

        GameManager.Instance.EndTurn();

    }
}
