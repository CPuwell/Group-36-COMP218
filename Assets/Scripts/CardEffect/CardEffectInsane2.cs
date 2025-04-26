using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane2 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】查看一名玩家的手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");

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

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            List<Card> cards = selectedTarget.GetCards();
            if (cards.Count > 0)
            {
                UIManager.Instance.ShowCardReveal(cards[0], selectedTarget.playerName, () =>
                {
                    currentPlayer.GoInsane(); // 理智效果后进入疯狂
                    GameManager.Instance.EndTurn();
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌");
                currentPlayer.GoInsane();
                GameManager.Instance.EndTurn();
            }
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】查看目标手牌 → 自己摸牌 → 弃一张");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供查看");

            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            List<Card> targetCards = selectedTarget.GetCards();

            if (targetCards.Count > 0)
            {
                UIManager.Instance.ShowCardReveal(targetCards[0], selectedTarget.playerName, () =>
                {
                    currentPlayer.DrawCard(GameManager.Instance.deck);

                    List<Card> myCards = currentPlayer.GetCards();
                    if (myCards.Count == 2)
                    {
                        // 弹出弃牌选择面板
                        UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                        {
                            currentPlayer.DiscardCard(cardToDiscard);
                            GameManager.Instance.EndTurn();
                        });
                    }
                    else
                    {
                        Debug.Log("手牌不足2张，无法弃牌");
                        GameManager.Instance.EndTurn();
                    }
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌");
                currentPlayer.DrawCard(GameManager.Instance.deck);
                GameManager.Instance.EndTurn();
            }
        });
    }
}
