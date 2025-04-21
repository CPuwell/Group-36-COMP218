using UnityEngine;

public class CardEffect4 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        currentPlayer.SetProtected(true);
        Debug.Log($"{currentPlayer.playerName} 现在受到保护，直到他下次回合开始");
        GameManager.Instance.EndTurn();
    }
}
//是否需要增加弹窗效果