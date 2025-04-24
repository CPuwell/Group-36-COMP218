using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane3 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】与目标玩家比牌，点数低者出局");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可以对决的目标玩家");
            return;
        }

        // 使用 UI 选人
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} 的手牌数值：{currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} 的手牌数值：{targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 出局（点数低）");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 出局（点数低）");
            }
            else
            {
                UIManager.Instance.ShowPopup("点数相同，无人出局");
            }

            currentPlayer.GoInsane(); // 理智后变疯
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】若目标未疯狂 → 淘汰；否则提示免疫");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可以指定的目标玩家");
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            var uiNotice = Object.FindFirstObjectByType<UIEliminationResult>(); // 推荐写法

            if (!selectedTarget.IsInsane())
            {
                selectedTarget.Eliminate();
                uiNotice?.Show("This player is not in a crazy state and has been defeated and eliminated by you!");
            }
            else
            {
                uiNotice?.Show("The player has gone crazy and is immune to this attack.");
            }

            GameManager.Instance.EndTurn();
        });
    }

}
