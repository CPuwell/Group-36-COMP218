using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform deckZone;
    public Sprite[] frontSprites;
    public Sprite backSprite;

    void Start()
    {
        for (int i = 0; i < frontSprites.Length; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, deckZone);
            CardUI cardUI = newCard.GetComponent<CardUI>();
            cardUI.SetCard(frontSprites[i], backSprite);
            cardUI.Flip(false); // Ä¬ÈÏ±³Ãæ³¯ÉÏ
        }
    }
}
