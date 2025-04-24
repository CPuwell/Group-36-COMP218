using UnityEngine;

public class CardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】你将在下回合前免疫其他卡牌效果");

        currentPlayer.SetProtected(true); // 设置保护
        currentPlayer.GoInsane();         // 理智打出后变为疯狂

        // 弹出说明面板，确认后结束回合
        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            noticeUI.Show(
                "You cannot be chosen as part of the effects of other players' cards until the start of your next turn.",
                () => GameManager.Instance.EndTurn()
            );
        }
        else
        {
            Debug.LogWarning("找不到 UICardEffectNotice，直接结束回合");
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】你将在本轮游戏结束前不会被淘汰");

        currentPlayer.SetImmortalThisRound(true); // 设置不死

        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            noticeUI.Show(
                "You will not be knocked out until the end of this round.",
                () => GameManager.Instance.EndTurn()
            );
        }
        else
        {
            Debug.LogWarning("找不到 UICardEffectNotice，直接结束回合");
            GameManager.Instance.EndTurn();
        }
    }
}
