using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string id;  // 牌的编号
    public string cardName;  // 牌的名称
    public Sprite frontSprite; // 卡牌正面图片
    public Sprite backSprite;  // 卡牌背面图片
    public bool isInsaneCard; // 是否是疯狂牌
    public string saneEffect; // 理智状态效果
    public string insaneEffect; // 疯狂状态效果
}
