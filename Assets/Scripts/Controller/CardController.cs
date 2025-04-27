using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // Card basic data (instances of the Card class, including id, name, type, value, etc.)
    private Player owner; // owner of caard

    /// <summary>
    /// Set the player to which the card belongs (set when the card is handed to the player)
    /// </summary>
    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    /// <summary>
    /// Call this function externally to play the cards
    /// </summary>
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("The card data or owner has not been set!");
            return;
        }

        Debug.Log($"¡¾{owner.playerName}¡¿play¡¾{cardData.cardName}¡¿");

        // Sort and process according to the card type
        if (cardData.isInsane)
        {
            HandleInsaneCard(); // insane Card Logic
        }
        else
        {
            HandleNormalCard(); // normal card logic
        }
    }

    /// <summary>
    /// Handle ordinary cards (using the IMainEffect interface)
    /// </summary>
    private void HandleNormalCard()
    {
        var effect = GetComponent<IMainEffect>();
        if (effect != null)
        {
            effect.ExecuteEffect(owner);
        }
        else
        {
            Debug.LogWarning($"normal card {cardData.cardName} The IMainEffect script is not mounted!");

        }
    }

    /// <summary>
    ///Handle crazy cards (using the IInsaneCard interface)
    /// </summary>
    private void HandleInsaneCard()
    {
        var insaneEffect = GetComponent<IInsaneCard>();
        if (insaneEffect == null)
        {
            Debug.LogWarning($"insane card {cardData.cardName} The IMainEffect script is not mounted!");
            return;
        }

        //The player is still in a rational state and can only play the rational effect of the crazy card
        if (!owner.IsInsane())
        {
            Debug.Log($"{owner.playerName} is a rational state and only rational effects can be used");
            insaneEffect.ExecuteSaneEffect(owner);
           
        }
        else
        {
            // The player is crazy and can choose to execute the crazy or rational effect
            Debug.Log($"{owner.playerName} is insane. A selection UI pops up");

            UIInsaneChoice.Instance.Show(
                onSane: () =>
                {
                    Debug.Log("Choose to implement normal effects");
                    insaneEffect.ExecuteSaneEffect(owner);
                    
                },
                onInsane: () =>
                {
                    Debug.Log("Choose to implement insane effects");
                    insaneEffect.ExecuteInsaneEffect(owner);
                    
                }
            );
        }
    }

    /// <summary>
    /// If the player is still alive, proceed to the next round. (Elimination will not continue.)
    /// </summary>
    private void EndTurnIfNeeded()
    {
        if (owner.IsAlive())
        {
            GameManager.Instance.EndTurn();
        }
    }
}
