using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // 卡牌名字
    public string cardId;          // 卡牌编号（支持如 "1", "1m"）
    public Sprite frontSprite;     // 正面图片
    public Sprite backSprite;      // 背面图片
    public int value;              // 卡牌数值（用于比较大小）

    public CardType cardType;      // 卡牌类型
    public string description;     // 描述文本
    public bool isInsane;          // 是否是疯狂牌

    public GameObject cardObject;  // 对应的实例化 GameObject（运行时赋值）
    public GameObject effectPrefab; // 卡牌效果预制体（可拖入不同脚本 prefab）

    public Card(string name, string id, Sprite front, Sprite back, CardType type, string desc, int value, bool isInsane, GameObject effectPrefab = null)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        cardType = type;
        description = desc;
        this.value = value;
        this.isInsane = isInsane;
        this.effectPrefab = effectPrefab;
    }

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");

        // 触发特效 prefab（如果有）
        if (effectPrefab != null)
        {
            GameObject.Instantiate(effectPrefab);
        }

        // 你也可以考虑通过接口调用对应逻辑：
        // var script = effectPrefab.GetComponent<ICardEffect>();
        // script?.Execute();
    }
}

public enum CardType
{
    Attack,
    Defense,
    Special,
    Neutral
}
