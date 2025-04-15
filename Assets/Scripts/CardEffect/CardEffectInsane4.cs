using UnityEngine;

public class CardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】你将在下回合前免疫其他卡牌效果");
        currentPlayer.SetProtected(true);
        currentPlayer.GoInsane(); // 理智效果结束后，进入insane状态
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】你将在本轮游戏结束前不会被淘汰");
        currentPlayer.SetImmortalThisRound(true);
        // 不需要额外进入 insane，因为疯狂效果默认只能在 insane 状态下使用
        GameManager.Instance.EndTurn();
    }
}
