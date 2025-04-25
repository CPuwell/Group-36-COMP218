using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // 卡牌基础数据（Card 类实例，含 id、名称、类型、value 等）
    private Player owner; // 此卡牌的持有者

    /// <summary>
    /// 设置卡牌所属玩家（在卡牌发到玩家手里时设置）
    /// </summary>
    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    /// <summary>
    /// 外部调用这个函数来打出卡牌
    /// </summary>
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("卡牌数据或拥有者未设置！");
            return;
        }

        Debug.Log($"【{owner.playerName}】打出了【{cardData.cardName}】");

        // 根据卡牌类型分流处理
        if (cardData.isInsane)
        {
            HandleInsaneCard(); // 疯狂牌逻辑
        }
        else
        {
            HandleNormalCard(); // 普通牌逻辑
        }
    }

    /// <summary>
    /// 处理普通卡牌（使用 IMainEffect 接口）
    /// </summary>
    private void HandleNormalCard()
    {
        var effect = GetComponent<IMainEffect>();
        if (effect != null)
        {
            effect.ExecuteEffect(owner);
        }
        else
        {
            Debug.LogWarning($"普通卡牌 {cardData.cardName} 没有挂载 IMainEffect 效果脚本！");

        }
    }

    /// <summary>
    /// 处理疯狂卡牌（使用 IInsaneCard 接口）
    /// </summary>
    private void HandleInsaneCard()
    {
        var insaneEffect = GetComponent<IInsaneCard>();
        if (insaneEffect == null)
        {
            Debug.LogWarning($"疯狂卡牌 {cardData.cardName} 没有挂载 IInsaneCard 效果脚本！");
            return;
        }

        // 玩家还处于理智状态，只能打出疯狂牌的理智效果
        if (!owner.IsInsane())
        {
            Debug.Log($"{owner.playerName} 是理智状态，只能使用理智效果");
            insaneEffect.ExecuteSaneEffect(owner);
           
        }
        else
        {
            // 玩家已疯狂，可以选择执行疯狂或理智效果
            Debug.Log($"{owner.playerName} 已疯狂，弹出选择 UI");

            UIInsaneChoice.Instance.Show(
                onSane: () =>
                {
                    Debug.Log("选择执行 理智效果");
                    insaneEffect.ExecuteSaneEffect(owner);
                    
                },
                onInsane: () =>
                {
                    Debug.Log("选择执行 疯狂效果");
                    insaneEffect.ExecuteInsaneEffect(owner);
                    
                }
            );
        }
    }

    /// <summary>
    /// 如果玩家还活着，就继续下一回合（淘汰就不继续了）
    /// </summary>
    private void EndTurnIfNeeded()
    {
        if (owner.IsAlive())
        {
            GameManager.Instance.EndTurn();
        }
    }
}
