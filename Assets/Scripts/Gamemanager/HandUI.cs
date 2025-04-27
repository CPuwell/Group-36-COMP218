using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    public List<CardUI> cardSlots; // Drag CardSlot1 and CardSlot2 into the Inspector
    private List<Card> currentHand; // Current hand cards

    public Sprite backSprite; // Back sprite used when flipping the card
    public GameManager gameManager; // Reference to your game logic class

    void Start()
    {
        for (int i = 0; i < cardSlots.Count; i++)
        {
            int index = i; // Capture the current index
            Button btn = cardSlots[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => PlayCard(index));
            }
        }
    }

    public void UpdateHandUI(List<Card> hand)
    {
        currentHand = hand;
        Debug.Log($"Updating hand UI, current hand size: {hand.Count}");
        Debug.Log($"Number of CardSlots: {cardSlots.Count}");
        for (int i = 0; i < cardSlots.Count; i++)
        {
            // 1. Clear all old objects
            foreach (Transform child in cardSlots[i].transform)
            {
                Destroy(child.gameObject);
            }

            // 2. Fill each cardSlots[i] with a new card
            if (i < hand.Count)
            {
                Card card = hand[i];
                if (card.cardObject == null)
                {
                    Debug.LogError($"The cardObject of hand card {card.cardName} is null!");
                    continue;
                }

                // --- Key Point 1: Re-clone a new cardObject ---
                GameObject newCardObj = GameObject.Instantiate(card.cardObject);
                newCardObj.transform.SetParent(cardSlots[i].transform, false);
                newCardObj.transform.localPosition = Vector3.zero;
                newCardObj.transform.localScale = Vector3.one;
                newCardObj.transform.localRotation = Quaternion.identity;
                newCardObj.SetActive(true);

                // --- Key Point 2: Ensure CardUI is rebound ---
                CardUI cardUI = newCardObj.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(card, gameManager.GetCurrentPlayer().Hand);
                    cardUI.Flip(true); // Face up
                }
                else
                {
                    Debug.LogError($"The new cardObject is missing the CardUI component!");
                }
            }
        }
    }

    public void PlayCard(int index)
    {
        gameManager.PlayCard(currentHand[index]);
    }
}
