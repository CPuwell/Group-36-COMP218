using UnityEngine;

public class CardEffect8 : MonoBehaviour
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} 打出了8号牌，自爆出局！");
        GameManager.Instance.EndTurn();
    }
}
