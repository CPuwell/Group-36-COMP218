using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // ͨ��CardԤ����
    public Transform deckZone;              // �ƿ�����
    public Sprite[] frontSprites;            // 0~8����ͨ����9~17��Insane��
    public Sprite backSprite;                // ͳһ�Ŀ���

    private List<GameObject> deck = new List<GameObject>(); // ��ǰ�ƶ�

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
            CreateCard(0); // 0��sprite��Card1
        }

        // ��� Card2 ~ Card5 ������
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                CreateCard(id);
            }
        }

        // ��� Card6 ~ Card8 ��һ��
        for (int id = 5; id <= 7; id++)
        {
            CreateCard(id);
        }

        // ��� InsaneCard0 ~ InsaneCard8 ��һ��
        for (int id = 8; id <= 16; id++)
        {
            CreateCard(id);
        }
    }

    void CreateCard(int spriteIndex)
    {
        GameObject newCard = Instantiate(cardPrefab, deckZone);
        CardUI cardUI = newCard.GetComponent<CardUI>();

        if (cardUI != null)
        {
            cardUI.SetCard(frontSprites[spriteIndex], backSprite);
            cardUI.Flip(false); // ��ʼ�Ǳ��泯��
        }

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
