using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // Card base data (instance of Card class, includes id, name, type, value, etc.)
    private Player owner; // The owner of this card

    /// <summary>
    /// Set the owner of the card (called when the card is dealt to a player)
    /// </summary>
    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    /// <summary>
    /// External call to play this card
    /// </summary>
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("Card data or owner is not set!");
            return;
        }

        Debug.Log($"[{owner.playerName}] played [{cardData.cardName}]");

        // Dispatch according to card type
        if (cardData.isInsane)
        {
            HandleInsaneCard(); // Insane card logic
        }
        else
        {
            HandleNormalCard(); // Normal card logic
        }
    }

    /// <summary>
    /// Handle normal card (uses IMainEffect interface)
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
            Debug.LogWarning($"Normal card {cardData.cardName} does not have an IMainEffect script attached!");
        }
    }

    /// <summary>
    /// Handle insane card (uses IInsaneCard interface)
    /// </summary>
    private void HandleInsaneCard()
    {
        var insaneEffect = GetComponent<IInsaneCard>();
        if (insaneEffect == null)
        {
            Debug.LogWarning($"Insane card {cardData.cardName} does not have an IInsaneCard script attached!");
            return;
        }

        // Player is still sane, can only use the sane effect
        if (!owner.IsInsane())
        {
            Debug.Log($"{owner.playerName} is sane and can only use the sane effect");
            insaneEffect.ExecuteSaneEffect(owner);
        }
        else
        {
            if (owner.isHuman)
            {
                // Player is insane, can choose between insane and sane effects
                Debug.Log($"{owner.playerName} is insane, showing choice UI");

                UIInsaneChoice.Instance.Show(
                    onSane: () =>
                    {
                        Debug.Log("Chose to use Sane Effect");
                        insaneEffect.ExecuteSaneEffect(owner);
                    },
                    onInsane: () =>
                    {
                        Debug.Log("Chose to use Insane Effect");
                        insaneEffect.ExecuteInsaneEffect(owner);
                    }
                );
            }
            else
            {
                bool chooseInsane = Random.value > 0.5f; // Randomly choose 50%

                if (chooseInsane)
                {
                    Debug.Log($"{owner.playerName} (AI) chose to use Insane Effect");
                    insaneEffect.ExecuteInsaneEffect(owner);
                }
                else
                {
                    Debug.Log($"{owner.playerName} (AI) chose to use Sane Effect");
                    insaneEffect.ExecuteSaneEffect(owner);
                }
            }
        }
    }
}
