using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // 卡牌名字
    public int cardId;             // 卡牌编号（用于识别或加载资源）
    public Sprite frontSprite;     // 正面图片
    public Sprite backSprite;      // 背面图片（一般都一样）
    public int value;          // 卡牌序号

    public CardType cardType;      // 卡牌类型（比如攻击、防御、特殊等）
    public string description;     // 卡牌描述或效果文本

    public Card(string name, int id, Sprite front, Sprite back, CardType type, string desc)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        cardType = type;
        description = desc;
    }

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");
        // TODO: 实现卡牌效果逻辑
    }

}

public enum CardType
{
    Attack,
    Defense,
    Special,
    Neutral
}
