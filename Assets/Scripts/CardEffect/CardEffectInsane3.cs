using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane3 : MonoBehaviour, InsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】与目标玩家比牌，点数低者出局");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有可以对决的目标玩家");
            return;
        }

        Player target = targets[0]; // TODO：替换为 UI 选人

        int currentValue = currentPlayer.GetHandValue();
        int targetValue = target.GetHandValue();

        Debug.Log($"{currentPlayer.playerName} 的手牌数值：{currentValue}");
        Debug.Log($"{target.playerName} 的手牌数值：{targetValue}");

        if (currentValue > targetValue)
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} 出局（点数低）");
        }
        else if (currentValue < targetValue)
        {
            currentPlayer.Eliminate();
            Debug.Log($"{currentPlayer.playerName} 出局（点数低）");
        }
        else
        {
            Debug.Log("点数相同，无人出局");
        }

        currentPlayer.GoInsane(); // 理智效果执行后进入insane状态
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】若目标玩家尚未疯狂，则直接出局");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有可以指定的目标玩家");
            return;
        }

        Player target = targets[0]; // TODO：替换为 UI 选人

        if (!target.IsInsane())
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} 仍处于理智状态，被疯狂效果淘汰！");
        }
        else
        {
            Debug.Log($"{target.playerName} 已经是疯狂状态，没有效果");
        }
        GameManager.Instance.EndTurn();
    }
}
