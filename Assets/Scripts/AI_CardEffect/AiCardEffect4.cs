using UnityEngine;

public class AiCardEffect4 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】你获得保护，直到下次回合开始，不能被其他玩家选中");

        // 设置玩家为 protected
        currentPlayer.SetProtected(true);

        // 弹出说明面板，点击确认后再执行回合结束
        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            noticeUI.Show(
                "You cannot be chosen as part of the effects of other players' cards until the start of your next turn.",
                () =>
                {
                    GameManager.Instance.EndTurn();
                }
            );
        }
        else
        {
            Debug.LogWarning("UICardEffectNotice 面板未找到，直接结束回合");
            GameManager.Instance.EndTurn();
        }
    }
}
