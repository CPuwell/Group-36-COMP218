using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane2 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�����鿴һ����ҵ�����");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ��鿴");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard(); 
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            List<Card> cards = selectedTarget.GetCards();
            if (cards.Count > 0)
            {
                UIManager.Instance.ShowCardReveal(cards[0], selectedTarget.playerName, () =>
                {
                    currentPlayer.GoInsane(); // ����Ч���������
                    GameManager.Instance.EndTurn();
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} û������");
                currentPlayer.GoInsane();
                GameManager.Instance.EndTurn();
            }
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����鿴Ŀ������ �� �Լ����� �� ��һ��");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ��鿴");

            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            List<Card> targetCards = selectedTarget.GetCards();

            if (targetCards.Count > 0)
            {
                UIManager.Instance.ShowCardReveal(targetCards[0], selectedTarget.playerName, () =>
                {
                    currentPlayer.DrawCard(GameManager.Instance.deck);

                    List<Card> myCards = currentPlayer.GetCards();
                    if (myCards.Count == 2)
                    {
                        // ��������ѡ�����
                        UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                        {
                            currentPlayer.DiscardCard(cardToDiscard);
                            GameManager.Instance.EndTurn();
                        });
                    }
                    else
                    {
                        Debug.Log("���Ʋ���2�ţ��޷�����");
                        GameManager.Instance.EndTurn();
                    }
                });
            }
            else
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} û������");
                currentPlayer.DrawCard(GameManager.Instance.deck);
                GameManager.Instance.EndTurn();
            }
        });
    }
}
