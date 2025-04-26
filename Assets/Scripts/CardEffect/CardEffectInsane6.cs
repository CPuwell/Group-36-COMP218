using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane6 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】与目标交换手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("无法选择玩家");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, target =>
        {
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

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】重新分配所有玩家手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);
        List<Player> playersWithCards = targets.FindAll(p => p.GetCards().Count > 0);

        if (playersWithCards.Count < 2)
        {
            UIManager.Instance.ShowPopup("没有足够的玩家参与重新分配（至少2人有牌）");
            
            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            GameManager.Instance.EndTurn();
            return;
        }
        
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

        Shuffle(collectedCards);

        for (int i = 0; i < playersWithCards.Count; i++)
        {
            playersWithCards[i].AddCard(collectedCards[i]);
            Debug.Log($"{playersWithCards[i].playerName} 获得了新手牌：{collectedCards[i].cardName}");
        }

        // 使用简单弹窗通知
        UIManager.Instance.ShowPopup("All the players' hand cards have been redistributed.");

        GameManager.Instance.EndTurn();
    }


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
