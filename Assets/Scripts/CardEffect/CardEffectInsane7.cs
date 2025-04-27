using UnityEngine;

public class CardEffectInsane7 : MonoBehaviour, IInsaneCard
{
    private Card thisCard;

    private void Awake()
    {
        CardController controller = GetComponent<CardController>();
        if (controller != null)
        {
            thisCard = controller.cardData;
        }
        else
        {
            Debug.LogError("CardEffectInsane7 找不到 CardController！");
        }
    }


    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】7号牌没有主动效果，仅受出牌限制控制（不能打出除非另一张牌大于4）");
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
            currentPlayer.WinRound();
            GameManager.Instance.DeclareWinner(currentPlayer);

            // 弹出提示并宣布胜利
            UIManager.Instance.ShowPopup("你另一张牌数值大于 4，疯狂效果触发！你赢得了本轮游戏！");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            Debug.Log("另一张手牌不大于4，效果无效");
            UIManager.Instance.ShowPopup("你的另一张手牌不大于 4，疯狂效果未触发");
            GameManager.Instance.EndTurn();
        }
    }
}
