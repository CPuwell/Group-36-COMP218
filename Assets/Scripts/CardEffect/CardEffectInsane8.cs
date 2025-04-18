using UnityEngine;

public class CardEffectInsane8 : MonoBehaviour, InsaneCard
{
    private void Awake()
    {
        // 无需做 thisCard 获取
    }

    // 理智效果什么都不用写，出局逻辑已在 RecordDiscard() 中统一处理
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】已由弃牌判定系统统一处理出局，无需在此重复判断");
        currentPlayer.GoInsane(); // 理智打出后仍进入疯狂状态
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】检查弃牌堆中疯狂牌数量...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} 弃牌堆中已有 {insaneDiscards} 张疯狂牌，触发疯狂胜利条件！");
            GameManager.Instance.DeclareWinner(currentPlayer); // 假设已设为 public
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                GameManager.Instance.EndTurn();
            }
            else
            {
                currentPlayer.Eliminate();
            }
        }
    }

}
