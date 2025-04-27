using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // Card name
    public string cardId;          // Card ID (supports formats like "1", "1m")
    public Sprite frontSprite;     // Front image
    public Sprite backSprite;      // Back image
    public int value;              // Card value (used for comparison)

    public string description;     // Description text
    public bool isInsane;           // Whether it is an insane card

    [System.NonSerialized]
    public GameObject cardObject;   // Instantiated GameObject (assigned at runtime)
    public GameObject effectPrefab; // Card effect prefab (different scripts can be attached)

    public Card(string name, string id, Sprite front, Sprite back, string desc, int value, bool isInsane, GameObject effectPrefab = null)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        description = desc;
        this.value = value;
        this.isInsane = isInsane;
        this.effectPrefab = effectPrefab;
    }
}
