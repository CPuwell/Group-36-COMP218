using UnityEngine;

public class CardEffectInsane8 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】已由弃牌判定系统统一处理出局，无需在此重复判断");
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】检查弃牌堆中疯狂牌数量...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} 弃牌堆中已有 {insaneDiscards} 张疯狂牌，触发疯狂胜利条件！");
            UIManager.Instance.ShowPopup("你打出了疯狂8号牌，弃牌堆中已有两张或以上疯狂牌！你赢得了整场游戏！");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                UIManager.Instance.ShowPopup("你暂时处于不死状态，虽然疯狂胜利条件未触发，但你不会被淘汰。");
                GameManager.Instance.EndTurn();
            }
            else
            {
                UIManager.Instance.ShowPopup("你打出了疯狂8号牌，但弃牌堆中疯狂牌不足两张。你被淘汰！");
                currentPlayer.Eliminate();
            }
        }
    }
}
