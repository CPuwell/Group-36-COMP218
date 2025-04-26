using UnityEngine;

public class AiCardEffect8 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} 打出了8号牌，自爆出局！");
        currentPlayer.Eliminate(); // 补充真正的淘汰逻辑
        GameManager.Instance.EndTurn();
    }
}
