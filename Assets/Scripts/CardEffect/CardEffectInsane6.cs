using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane6 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】与你选择的一名玩家交换手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("没有可以交换的目标玩家");
            return;
        }

        Player target = targets[0]; // TODO：UI选择目标

        Card myCard = currentPlayer.RemoveCard();
        Card theirCard = target.RemoveCard();

        if (myCard != null && theirCard != null)
        {
            currentPlayer.AddCard(theirCard);
            target.AddCard(myCard);

            Debug.Log($"{currentPlayer.playerName} 与 {target.playerName} 交换了手牌");
        }
        else
        {
            Debug.Log("交换失败，其中一人没有手牌");
        }

        currentPlayer.GoInsane(); // 理智效果执行后变为insane
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】重新分配所有玩家的手牌");

        // 包含自己，筛选所有活着、未被保护、手上有牌的玩家
        List<Player> targets = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);
        List<Player> playersWithCards = targets.FindAll(p => p.GetCards().Count > 0);

        if (playersWithCards.Count < 2)
        {
            Debug.Log("没有足够的玩家参与重新分配（至少2人有牌）");
            return;
        }

        // 收集所有玩家手牌
        List<Card> collectedCards = new List<Card>();
        foreach (Player player in playersWithCards)
        {
            Card card = player.RemoveCard();
            if (card != null)
            {
                collectedCards.Add(card);
                Debug.Log($"收集了 {player.playerName} 的手牌：{card.cardName}");
            }
        }

        // 随机打乱牌
        Shuffle(collectedCards);

        // 分配给原玩家（顺序打乱）
        for (int i = 0; i < playersWithCards.Count; i++)
        {
            playersWithCards[i].AddCard(collectedCards[i]);
            Debug.Log($"{playersWithCards[i].playerName} 获得了新手牌：{collectedCards[i].cardName}");
        }
        GameManager.Instance.EndTurn();
    }

    // Fisher–Yates 洗牌算法
    private void Shuffle(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
