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

        // UI 选人
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} 的手牌数值：{currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} 的手牌数值：{targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 出局（点数较低）");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 出局（点数较低）");
            }
            else
            {
                UIManager.Instance.ShowPopup("平局！无人出局");
            }

            currentPlayer.GoInsane(); // 理智效果后进入疯狂状态
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
            if (!selectedTarget.IsInsane())
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 未进入疯狂状态，被淘汰！");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 已处于疯狂状态，免疫淘汰");
            }

            GameManager.Instance.EndTurn();
        });
    }
}
