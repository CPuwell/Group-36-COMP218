using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // 卡牌名字
    public string cardId;          // 卡牌编号（支持如 "1", "1m"）
    public Sprite frontSprite;     // 正面图片
    public Sprite backSprite;      // 背面图片
    public int value;              // 卡牌数值（用于比较大小）


    public string description;     // 描述文本
    public bool isInsane;          // 是否是疯狂牌

    public GameObject cardObject;  // 对应的实例化 GameObject（运行时赋值）
    public GameObject effectPrefab; // 卡牌效果预制体（可拖入不同脚本 prefab）

    public Card(string name, string id, Sprite front, Sprite back, string desc, int value, bool isInsane, GameObject effectPrefab = null)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        
        description = desc;
        this.value = value;
        this.isInsane = isInsane;
        this.effectPrefab = effectPrefab;
    }

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");

        if (effectPrefab != null)
        {
            Debug.Log($"Instantiating effect prefab for {cardName}.");
            GameObject effectInstance = GameObject.Instantiate(effectPrefab);

            // 尝试获取常见的UI效果类型，并调用Show
            if (effectInstance.TryGetComponent<UICardEffectNotice>(out var notice))
            {
                notice.Show(description, () =>
                {
                    Debug.Log($"{cardName} 弹窗确认完成！");
                });
            }
            else if (effectInstance.TryGetComponent<UIGuess>(out var guess))
            {
                // 猜牌逻辑（需要传目标玩家列表和回调）
                guess.Show(GameManager.Instance.GetAvailableTargets(GameManager.Instance.GetCurrentPlayer()), (target, guessNumber) =>
                {
                    Debug.Log($"猜测完成，目标：{target.playerName}，数字：{guessNumber}");
                });
            }
            else if (effectInstance.TryGetComponent<UIPlayerSelect>(out var select))
            {
                select.Show(GameManager.Instance.GetAvailableTargets(GameManager.Instance.GetCurrentPlayer()), (target) =>
                {
                    Debug.Log($"选择了玩家：{target.playerName}");
                });
            }
            else
            {
                Debug.LogWarning($"未知卡牌效果Prefab，未能识别脚本类型！");
            }
        }
        else
        {
            Debug.LogWarning($"卡牌 {cardName} 没有挂载效果Prefab！");
        }
    }

}
