using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public Image frontImage;
    public Image backImage;

    private Card cardData;              // The card data currently bound to this UI
    private bool isFaceUp = false;
    private Hand owningHand;             // Reference to the owning hand (logic layer)

    // Set card visuals and data
    public void SetCard(Card card, Hand hand)
    {
        Debug.Log($"Set card: {card.cardName}");
        cardData = card;
        owningHand = hand; // Bind hand reference

        frontImage.sprite = card.frontSprite;
        backImage.sprite = card.backSprite;

        Flip(false); // Initially face down
    }

    // Flip the card: true = face up, false = face down
    public void Flip(bool faceUp)
    {
        isFaceUp = faceUp;
        frontImage.gameObject.SetActive(faceUp);
        backImage.gameObject.SetActive(!faceUp);
    }

    // Handle click events (double-click to play the card)
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on card: {(cardData != null ? cardData.cardName : "null")}");

        if (!IsMyTurn())
        {
            Debug.LogWarning("It is not your turn. You cannot play a card now!");
            return;
        }

        if (owningHand != null && cardData != null)
        {
            owningHand.SelectCard(cardData);
        }
        else
        {
            Debug.LogWarning("CardUI clicked but owningHand or cardData is null!");
        }
    }

    // Delegate method: called when this card is selected
    public void SetSelected(bool selected)
    {
        GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }

    private bool IsMyTurn()
    {
        if (GameManager.Instance == null) return false;

        Player currentPlayer = GameManager.Instance.GetCurrentPlayer();
        return currentPlayer != null && currentPlayer.isHuman;
    }
}
