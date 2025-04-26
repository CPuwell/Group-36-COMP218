using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane2 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【正常效果】查看一名玩家的手牌");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可以查看的玩家");
            return;
        }

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
            {
                List<Card> cards = selectedTarget.GetCards();
                if (cards.Count > 0)
                {
                    UIManager.Instance.ShowCardReveal(cards[0], selectedTarget.playerName, () =>
                    {
                        currentPlayer.GoInsane(); // 正常效果触发疯狂
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
        else
        {
            // AI player logic
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            List<Card> cards = selectedTarget.GetCards();
            
            UIManager.Instance.Log($"AI {currentPlayer.playerName} 查看了 {selectedTarget.playerName} 的手牌");
            
            if (cards.Count > 0)
            {
                // AI doesn't need to actually see the card, but we log it for game tracking
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 看到了 {selectedTarget.playerName} 的牌: {cards[0].cardName}");
                currentPlayer.GoInsane();
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌");
                currentPlayer.GoInsane();
            }
            
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】查看目标手牌 + 自己抽牌 + 弃一张");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可查看的目标玩家");
            return;
        }

        if (currentPlayer.isHuman)
        {
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
                            // 人类玩家选择弃牌
                            UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                            {
                                currentPlayer.DiscardCard(cardToDiscard);
                                GameManager.Instance.EndTurn();
                            });
                        }
                        else
                        {
                            Debug.Log("手牌不是2张，无法弃牌");
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
        else
        {
            // AI player logic for insane mode
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            List<Card> targetCards = selectedTarget.GetCards();
            
            UIManager.Instance.Log($"AI {currentPlayer.playerName} 查看了 {selectedTarget.playerName} 的手牌");
            
            if (targetCards.Count > 0)
            {
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 看到了 {selectedTarget.playerName} 的牌: {targetCards[0].cardName}");
                
                // AI draws a card
                currentPlayer.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 抽了一张牌");
                
                List<Card> myCards = currentPlayer.GetCards();
                if (myCards.Count == 2)
                {
                    // AI discards a card (random strategy)
                    Card cardToDiscard = myCards[Random.Range(0, myCards.Count)];
                    currentPlayer.DiscardCard(cardToDiscard);
                    UIManager.Instance.Log($"AI {currentPlayer.playerName} 弃掉了 {cardToDiscard.cardName}");
                }
                else
                {
                    Debug.Log("AI 手牌不是2张，无法弃牌");
                }
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 没有手牌");
                currentPlayer.DrawCard(GameManager.Instance.deck);
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 抽了一张牌");
            }
            
            GameManager.Instance.EndTurn();
        }
    }
}