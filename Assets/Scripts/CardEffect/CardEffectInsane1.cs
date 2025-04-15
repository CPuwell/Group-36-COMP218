using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("执行【理智】效果：猜牌，猜中则目标出局，不能猜1");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            Debug.Log("没有可以选择的玩家");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人
        int guess = 5; // TODO: 替换为 UI 输入（不能为1）

        if (guess == 1)
        {
            Debug.Log("不能猜1号牌！");
            return;
        }

        if (target.GetHandValue() == guess)
        {
            target.Eliminate();
            Debug.Log($"猜中！{target.playerName} 出局");
        }
        else
        {
            Debug.Log("猜错了，没有效果");
        }

        // 理智效果执行完，进入 insane 状态
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("执行【疯狂】效果：先检测对方是否是1号牌，否则再猜一次");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            Debug.Log("没有可以选择的玩家");
            return;
        }

        Player target = targets[0]; // TODO: 替换为 UI 选人

        int handValue = target.GetHandValue();
        if (handValue == 1)
        {
            target.Eliminate();
            Debug.Log($"{target.playerName} 手牌是1号，直接出局！");
        }
        else
        {
            Debug.Log($"{target.playerName} 不是1号牌，可以再猜一次");

            int guess = 6; // TODO: 替换为 UI 输入（不能为1）

            if (guess == 1)
            {
                Debug.Log("不能猜1号牌！");
                return;
            }

            if (handValue == guess)
            {
                target.Eliminate();
                Debug.Log($"猜中！{target.playerName} 出局");
            }
            else
            {
                Debug.Log("再次猜错了，没有效果");
            }
        }
        GameManager.Instance.EndTurn();
    }
}
