using UnityEngine;

public class AICardEffectInsane7 : MonoBehaviour, IInsaneCard
{
    private Card thisCard;

    private void Awake()
    {
        thisCard = GetComponent<Card>();
    }

    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【正常效果】7号牌没有主动效果，仅含出牌限制功能（不能打出除非另一张牌大于4）");
        
        // No special action needed for human or AI players - just go insane and end turn
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】如果你另一张牌大于4，你直接赢得本轮");

        Card otherCard = currentPlayer.GetOtherCard(thisCard);
        if (otherCard == null)
        {
            Debug.Log("没有找到另一张手牌，效果无效");
            UIManager.Instance.ShowPopup("你的另一张牌无法识别，效果无效");
            GameManager.Instance.EndTurn();
            return;
        }

        Debug.Log($"你的另一张手牌是：{otherCard.cardName}（数值 {otherCard.value}）");

        if (otherCard.value > 4)
        {
            Debug.Log($"{currentPlayer.playerName} 成功触发疯狂7号牌效果，立即赢得本轮！");
            
            if (currentPlayer.isHuman)
            {
                UIManager.Instance.ShowPopup("你另一张牌数值大于 4，疯狂效果触发！你赢得了本轮游戏！");
            }
            else
            {
                // AI player victory
                UIManager.Instance.ShowPopup($"AI {currentPlayer.playerName} 另一张牌数值大于 4，疯狂效果触发！AI 赢得了本轮游戏！");
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 触发了疯狂7号牌胜利条件");
            }
            
            currentPlayer.WinRound();
            GameManager.Instance.EndRound();
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            Debug.Log("另一张手牌不大于4，效果无效");
            
            if (currentPlayer.isHuman)
            {
                UIManager.Instance.ShowPopup("你的另一张手牌不大于 4，疯狂效果未触发");
            }
            else
            {
                UIManager.Instance.ShowPopup($"AI {currentPlayer.playerName} 的另一张手牌不大于 4，疯狂效果未触发");
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 的疯狂7号牌效果失败");
            }
            
            GameManager.Instance.EndTurn();
        }
    }
}