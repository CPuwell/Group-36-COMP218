using UnityEngine;

public class CardEffectInsane7 : MonoBehaviour, IInsaneCard
{
    private Card thisCard; // 当前这张卡牌

    private void Awake()
    {
        thisCard = GetComponent<Card>();
    }

    /// <summary>
    /// 理智效果：你不能主动打这张牌，除非另一张牌 > 4（由 Hand.CanPlayCard 控制）
    /// 此处仅用于日志记录
    /// </summary>
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】7号牌没有主动效果，仅受出牌限制控制（不能打出除非另一张牌大于4）");
        currentPlayer.GoInsane(); // 打出后依然进入疯狂状态
        GameManager.Instance.EndTurn();
    }

    /// <summary>
    /// 疯狂效果：若另一张牌大于4，立即赢得本轮游戏
    /// </summary>
    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】如果你另一张牌大于4，你直接赢得本轮");

        Card otherCard = currentPlayer.GetOtherCard(thisCard);
        if (otherCard == null)
        {
            Debug.Log("没有找到另一张手牌，效果无效");
            GameManager.Instance.EndTurn();
            return;
        }

        Debug.Log($"你的另一张手牌是：{otherCard.cardName}（数值 {otherCard.value}）");

        if (otherCard.value > 4)
        {
            Debug.Log($"{currentPlayer.playerName} 成功触发疯狂7号牌效果，立即赢得本轮！");
            currentPlayer.winRound(); // 累加胜利回合
            GameManager.Instance.EndRound();
            GameManager.Instance.DeclareWinner(currentPlayer); // 立即宣布胜者（你可以将其设为 public）
        }
        else
        {
            Debug.Log("另一张手牌不大于4，效果无效");
        }
    }
}
