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

    [System.NonSerialized]
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

    

}
