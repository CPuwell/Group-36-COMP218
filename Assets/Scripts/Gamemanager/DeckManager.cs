using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // ͨ��CardԤ����
    public Transform deckZone;              // �ƿ�����
    public Sprite[] frontSprites;            // 0~8����ͨ����9~17��Insane��
    public Sprite backSprite;                // ͳһ�Ŀ���

    private List<GameObject> deck = new List<GameObject>(); // ��ǰ�ƶ�
    

    public Deck logicDeck = new Deck(); // �߼��ƶ�
    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    public void InitializeDeck()
    {
        logicDeck = new Deck(); // ���� new һ���µ� Deck

        // ���֮ǰ����
        foreach (Transform child in deckZone)
        {
            if (child.gameObject.name == "DeckTopCardImage") continue; //  ����DeckTopCardImage
            Destroy(child.gameObject);
        }
        foreach (GameObject card in deck)
        {
            Destroy(card);
        }
        deck.Clear();

        // ��� Card1 ����
        for (int i = 0; i < 5; i++)
        {
            Card c = CreateCardData("Card1", "1", 1, frontSprites[0], false);
            CreateCard(c);
        }

        // ��� Card2 ~ Card5 ������
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
                CreateCard(c);
            }
        }

        // ��� Card6 ~ Card8 ��һ��
        for (int id = 5; id <= 7; id++)
        {
            Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
            CreateCard(c);
        }

        // ��� InsaneCard0 ~ InsaneCard8 ��һ��
        for (int id = 8; id <= 16; id++)
        {
            Card c = CreateCardData($"InsaneCard{id - 7}", $"{id - 7}m", id + 1, frontSprites[id], true);
            CreateCard(c);
        }

        Debug.Log($"�ƶѳ�ʼ����ɣ����� {deck.Count} ���ơ�");
    }

    void CreateCard(Card cardData)
    {

        // 1. ����һ�ſ���GameObject
        GameObject newCard = Instantiate(cardPrefab, deckZone); // cardPrefab ������ǰ���õĿ�ƬԤ���壬deckZone���ƶ�����
        cardData.cardObject = newCard; // ������½��� GameObject ��ֵ�� Card �� cardObject ����
        // 2. �ҵ������Ƶ�CardUI���
        CardUI cardUI = newCard.GetComponent<CardUI>();

        if (cardUI != null)
        {
            // 3. ��CardData����CardUI����CardUI�Լ���ͼƬ���߼�
            cardUI.SetCard(cardData, null); //����� Hand �� null����Ϊ��Deck�׶Σ��ƻ�û�����ƣ�����Ҫ��Hand��
            cardUI.Flip(false); // ��ʼ���ţ����泯�ϣ�
        }
        else
        {
            Debug.LogError("CreateCardʧ�ܣ��½��������Ҳ��� CardUI �ű���");
        }

        // 4. ���������Ƽ���deck��
        deck.Add(newCard);
        logicDeck.AddCard(cardData); // ���������ݼ����߼��ƶ�
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

        // ���Ų㼶
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("Deck shuffled!");
    }

    // ��������һ�����ƽӿ�
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
        // ������Ч prefab���� Resources/Effects �ļ����У�
        GameObject effect = Resources.Load<GameObject>($"Effects/CardEffect{value}");

        Card card = new Card(
            name,
            id,
            front,
            backSprite,
            
            "����", // �������չ�����߼�
            value,
            isInsane,
            effect
        );

        return card;
    }

}
