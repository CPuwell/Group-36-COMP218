using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // ͨ��CardԤ����
    public Transform deckZone;              // �ƿ�����
    public Sprite[] frontSprites;            // 0~8����ͨ����9~17��Insane��
    public Sprite backSprite;                // ͳһ�Ŀ���

    private List<GameObject> deck = new List<GameObject>(); // ��ǰ�ƶ�
    public List<Card> allCardData; // ��������

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    void InitializeDeck()
    {
        // ���֮ǰ����
        foreach (Transform child in deckZone)
        {
            Destroy(child.gameObject);
        }
        deck.Clear();

        // ��� Card1 ����
        for (int i = 0; i < 5; i++)
        {
            CreateCard(allCardData[0]); // 0��sprite��Card1
        }

        // ��� Card2 ~ Card5 ������
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                CreateCard(allCardData[id]);
            }
        }

        // ��� Card6 ~ Card8 ��һ��
        for (int id = 5; id <= 7; id++)
        {
            CreateCard(allCardData[id]);
        }

        // ��� InsaneCard0 ~ InsaneCard8 ��һ��
        for (int id = 8; id <= 16; id++)
        {
            CreateCard(allCardData[id]);
        }
    }

    void CreateCard(Card cardData)
    {
        // 1. ����һ�ſ���GameObject
        GameObject newCard = Instantiate(cardPrefab, deckZone); // cardPrefab ������ǰ���õĿ�ƬԤ���壬deckZone���ƶ�����

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
}
