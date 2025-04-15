using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // 卡牌数据（包含 cardName、cardId、isInsane 等）
    private Player owner; // 此卡牌属于的玩家（出牌时传入）

    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    // 出牌接口（由 Hand 或 UI 调用）
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("卡牌数据或持有者未设置！");
            return;
        }

        Debug.Log($"【{owner.playerName}】打出了【{cardData.cardName}】");

        // 是 Insane Card（疯狂牌）？
        if (cardData.isInsane)
        {
            var effectScript = GetComponent<IInsaneCard>();
            if (effectScript == null)
            {
                Debug.LogWarning("疯狂牌缺少 IInsaneCard 效果脚本！");
                return;
            }

            if (!owner.IsInsane())
            {
                // 理智状态，只能执行理智效果 → 然后变为 insane
                effectScript.ExecuteSaneEffect(owner);
                owner.GoInsane();
                GameManager.Instance.EndTurn();
            }
            else
            {
                // 疯狂状态，弹出选择 UI
                UIInsaneChoice.Instance.Show(
                    onSane: () =>
                    {
                        effectScript.ExecuteSaneEffect(owner);
                        GameManager.Instance.EndTurn();
                    },
                    onInsane: () =>
                    {
                        effectScript.ExecuteInsaneEffect(owner);
                        GameManager.Instance.EndTurn();
                    }
                );
            }
        }
        else
        {
            // 普通卡牌处理
            var effect = GetComponent<IMainEffect>();
            if (effect != null)
            {
                effect.ExecuteEffect(owner);
            }
            else
            {
                Debug.Log("普通卡牌没有挂载效果脚本，跳过执行");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
