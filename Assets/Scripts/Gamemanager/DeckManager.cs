using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // 通用Card预制体
    public Transform deckZone;              // 牌库区域
    public Sprite[] frontSprites;            // 0~8是普通卡，9~17是Insane卡
    public Sprite backSprite;                // 统一的卡背

    private List<GameObject> deck = new List<GameObject>(); // 当前牌堆
    

    public Deck logicDeck = new Deck(); // 逻辑牌堆
    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    public void InitializeDeck()
    {
        logicDeck = new Deck(); // 重新 new 一个新的 Deck

        // 清空之前的牌
        foreach (Transform child in deckZone)
        {
            if (child.gameObject.name == "DeckTopCardImage") continue; //  保留DeckTopCardImage
            Destroy(child.gameObject);
        }
        foreach (GameObject card in deck)
        {
            Destroy(card);
        }
        deck.Clear();

        // 添加 Card1 五张
        for (int i = 0; i < 5; i++)
        {
            Card c = CreateCardData("Card1", "1", 1, frontSprites[0], false);
            CreateCard(c);
        }

        // 添加 Card2 ~ Card5 各两张
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
                CreateCard(c);
            }
        }

        // 添加 Card6 ~ Card8 各一张
        for (int id = 5; id <= 7; id++)
        {
            Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
            CreateCard(c);
        }

        // 添加 InsaneCard0 ~ InsaneCard8 各一张
        for (int id = 8; id <= 16; id++)
        {
            Card c = CreateCardData($"InsaneCard{id - 7}", $"{id - 7}m", id + 1, frontSprites[id], true);
            CreateCard(c);
        }

        Debug.Log($"牌堆初始化完成，共有 {deck.Count} 张牌。");
    }

    void CreateCard(Card cardData)
    {

        // 1. 创建一张卡牌GameObject
        GameObject newCard = Instantiate(cardPrefab, deckZone); // cardPrefab 是你提前做好的卡片预制体，deckZone是牌堆区域
        cardData.cardObject = newCard; // 这里把新建的 GameObject 赋值给 Card 的 cardObject 属性
        // 2. 找到这张牌的CardUI组件
        CardUI cardUI = newCard.GetComponent<CardUI>();

        if (cardUI != null)
        {
            // 3. 把CardData传给CardUI，让CardUI自己绑定图片和逻辑
            cardUI.SetCard(cardData, null); //这里的 Hand 传 null，因为在Deck阶段，牌还没进手牌（不需要绑定Hand）
            cardUI.Flip(false); // 初始盖着（背面朝上）
        }
        else
        {
            Debug.LogError("CreateCard失败：新建的牌上找不到 CardUI 脚本！");
        }

        // 4. 将创建的牌加入deck中
        deck.Add(newCard);
        logicDeck.AddCard(cardData); // 将卡牌数据加入逻辑牌堆
    }



    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        // 重排层级
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("Deck shuffled!");
    }

    // 可以增加一个抽牌接口
    public GameObject DrawCard()
    {
        if (deck.Count > 0)
        {
            GameObject drawnCard = deck[0];
            deck.RemoveAt(0);
            return drawnCard;
        }
        else
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }

    }
    Card CreateCardData(string name, string id, int value, Sprite front, bool isInsane)
    {
        // 加载特效 prefab（从 Resources/Effects 文件夹中）
        GameObject effect = Resources.Load<GameObject>($"Effects/CardEffect{value}");

        Card card = new Card(
            name,
            id,
            front,
            backSprite,
            
            "描述", // 你可以拓展描述逻辑
            value,
            isInsane,
            effect
        );

        return card;
    }

}
