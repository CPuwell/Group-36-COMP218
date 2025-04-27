using UnityEngine;

public class AICardEffectInsane7 : MonoBehaviour, IInsaneCard
{
    private Card thisCard;

    private void Awake()
    {
        CardController controller = GetComponent<CardController>();
        if (controller != null)
        {
            thisCard = controller.cardData;
        }
        else
        {
            Debug.LogError("CardEffectInsane7: CardController not found!");
        }
    }

    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("[Sane Effect] Card 7 has no active effect. It only restricts play (cannot be played unless the other card's value is greater than 4).");

        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("[Insane Effect] If your other card's value is greater than 4, you win the round immediately.");

        Card otherCard = currentPlayer.GetOtherCard(thisCard);
        if (otherCard == null)
        {
            Debug.Log("No other card found. Effect invalid.");
            UIManager.Instance.ShowPopup("Your other card could not be identified. Effect invalid.");
            GameManager.Instance.EndTurn();
            return;
        }

        Debug.Log($"Your other card is: {otherCard.cardName} (Value: {otherCard.value})");

        if (otherCard.value > 4)
        {
            Debug.Log($"{currentPlayer.playerName} triggered the insane Card 7 effect and wins the round!");

            if (currentPlayer.isHuman)
            {
                UIManager.Instance.ShowPopup("Your other card's value is greater than 4! Insane effect triggered! You win the round!");
            }
            else
            {
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName}'s other card value is greater than 4. Insane effect triggered! {currentPlayer.playerName} wins the round!");
                UIManager.Instance.Log($"AI {currentPlayer.playerName} triggered the insane Card 7 victory condition.");
            }

            currentPlayer.WinRound();
            GameManager.Instance.EndTurn();
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            Debug.Log("Other card value is not greater than 4. Effect did not trigger.");

            if (currentPlayer.isHuman)
            {
                UIManager.Instance.ShowPopup("Your other card's value is not greater than 4. Insane effect not triggered.");
            }
            else
            {
                UIManager.Instance.ShowPopup($"AI {currentPlayer.playerName}'Insane effect not triggered.");
                UIManager.Instance.Log($"AI {currentPlayer.playerName}'s insane Card 7 effect failed.");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
