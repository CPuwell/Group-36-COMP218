using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;           // General Card prefab
    public Transform deckZone;              // Deck area
    public Sprite[] frontSprites;            // Front sprites: 0-8 normal cards, 9-17 insane cards
    public Sprite backSprite;                // Unified card back
    public Transform specialCardZone;        // Special zone for Card 0

    private List<GameObject> deck = new List<GameObject>(); // Current deck
    public Deck logicDeck = new Deck(); // Logical deck

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    public void InitializeDeck()
    {
        logicDeck = new Deck(); // Reset logic deck

        // Clear previous cards
        foreach (Transform child in deckZone)
        {
            if (child.gameObject.name == "DeckTopCardImage") continue; // Keep DeckTopCardImage
            Destroy(child.gameObject);
        }
        foreach (GameObject card in deck)
        {
            Destroy(card);
        }
        deck.Clear();

        // Add five copies of Card1
        for (int i = 0; i < 5; i++)
        {
            Card c = CreateCardData("Card1", "1", 1, frontSprites[0], false);
            CreateCard(c);
        }

        // Add two copies of Card2 ~ Card5
        for (int id = 1; id <= 4; id++)
        {
            for (int j = 0; j < 2; j++)
            {
                Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
                CreateCard(c);
            }
        }

        // Add one copy of Card6 ~ Card8
        for (int id = 5; id <= 7; id++)
        {
            Card c = CreateCardData($"Card{id + 1}", $"{id + 1}", id + 1, frontSprites[id], false);
            CreateCard(c);
        }

        // Add one copy of InsaneCard0 ~ InsaneCard8
        for (int id = 8; id <= 16; id++)
        {
            Card c = CreateCardData($"InsaneCard{id - 8}", $"{id - 8}m", id - 8, frontSprites[id], true);
            CreateCard(c);
        }

        Debug.Log($"Deck initialized. Total cards: {deck.Count}");
    }

    void CreateCard(Card cardData)
    {
        // 1. Instantiate a new Card GameObject
        GameObject newCard = Instantiate(cardPrefab, deckZone);
        cardData.cardObject = newCard;

        // 2. Set up CardUI
        CardUI cardUI = newCard.GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.SetCard(cardData, null); // No Hand yet during deck setup
            cardUI.Flip(false); // Face down initially
        }
        else
        {
            Debug.LogError("CreateCard failed: CardUI component not found!");
        }

        CardController controller = newCard.GetComponent<CardController>();
        if (controller == null)
        {
            controller = newCard.AddComponent<CardController>();
        }
        controller.cardData = cardData;

        AttachCardEffect(newCard, cardData);

        if (cardData.cardId == "0m")
        {
            if (specialCardZone != null)
            {
                newCard.transform.SetParent(specialCardZone, false);
                newCard.transform.localPosition = Vector3.zero;
                newCard.transform.localScale = Vector3.one;
                Debug.Log("Special Card 0 placed into specialCardZone.");
            }
            else
            {
                Debug.LogWarning("specialCardZone not assigned. Card 0 remains in deckZone!");
            }
        }
        else
        {
            deck.Add(newCard);
            logicDeck.AddCard(cardData);
        }
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

        // Reorder hierarchy
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.SetSiblingIndex(i);
        }

        Debug.Log("Deck shuffled!");
    }

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
        // Load effect prefab from Resources/Effects folder
        GameObject effect = Resources.Load<GameObject>($"Effects/CardEffect{value}");

        Card card = new Card(
            name,
            id,
            front,
            backSprite,
            "Description", // You can expand this later
            value,
            isInsane,
            effect
        );

        return card;
    }

    void AttachCardEffect(GameObject card, Card cardData)
    {
        string scriptName;

        if (cardData.isInsane)
        {
            scriptName = $"AICardEffectInsane{cardData.value}";
        }
        else
        {
            scriptName = $"AiCardEffect{cardData.value}";
        }

        System.Type effectType = System.Type.GetType(scriptName);

        if (effectType != null)
        {
            card.AddComponent(effectType);
            Debug.Log($"Successfully attached script {scriptName} to {cardData.cardName}");
        }
        else
        {
            Debug.LogWarning($"Script {scriptName} not found. Skipped attaching to {cardData.cardName}");
        }
    }
}
