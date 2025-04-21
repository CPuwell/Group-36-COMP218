using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // 通用Card预制体
    public Transform deckZone;              // 牌库区域
    public Sprite[] frontSprites;            // 0~8是普通卡，9~17是Insane卡
    public Sprite backSprite;                // 统一的卡背

    private List<GameObject> deck = new List<GameObject>(); // 当前牌堆
    public List<Card> allCardData; // 卡牌数据

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    void InitializeDeck()
    {
        // 清空之前的牌
        foreach (Transform child in deckZone)
        {
            Destroy(child.gameObject);
        }
        deck.Clear();

        // 添加 Card1 五张
        for (int i = 0; i < 5; i++)
        {
            CreateCard(allCardData[0]); // 0号sprite是Card1
        }

        // 添加 Card2 ~ Card5 各两张
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                CreateCard(allCardData[id]);
            }
        }

        // 添加 Card6 ~ Card8 各一张
        for (int id = 5; id <= 7; id++)
        {
            CreateCard(allCardData[id]);
        }

        // 添加 InsaneCard0 ~ InsaneCard8 各一张
        for (int id = 8; id <= 16; id++)
        {
            CreateCard(allCardData[id]);
        }
    }

    void CreateCard(Card cardData)
    {
        // 1. 创建一张卡牌GameObject
        GameObject newCard = Instantiate(cardPrefab, deckZone); // cardPrefab 是你提前做好的卡片预制体，deckZone是牌堆区域

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
}
