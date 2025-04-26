using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane5 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч����Ŀ�����Ʋ���һ��");

        List<Player> targets = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û����ЧĿ��");
            GameManager.Instance.EndTurn();
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            Card oldCard = selectedTarget.RemoveCard();
            if (oldCard != null)
            {
                selectedTarget.DiscardCard(oldCard);
            }

            selectedTarget.DrawCard(GameManager.Instance.deck);

            UIManager.Instance.ShowPopup($"{selectedTarget.playerName} ���Ʋ�����һ������");

            currentPlayer.GoInsane(); // ����Ч���������״̬
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����͵ȡĿ�� �� ��һ�� �� ���Է�ǿ�����0����");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û����ЧĿ��");
            return;
        }

        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            Card stolenCard = selectedTarget.RemoveCard();
            if (stolenCard == null)
            {
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} û�����ƣ�͵��ʧ��");
                GameManager.Instance.EndTurn();
                return;
            }

            currentPlayer.AddCard(stolenCard);
            Debug.Log($"{currentPlayer.playerName} ͵ȡ�� {selectedTarget.playerName} �����ƣ�{stolenCard.cardName}");

            List<Card> myCards = currentPlayer.GetCards();
            if (myCards.Count == 2)
            {
                UIManager.Instance.ShowDiscardSelector(myCards[0], myCards[1], cardToDiscard =>
                {
                    currentPlayer.DiscardCard(cardToDiscard);

                    // ����Ŀ�� 0 ����
                    GameManager.Instance.GiveSpecificCardToPlayer(selectedTarget, "0");

                    // չʾ UI ��ʾĿ���� 0 ����
                    UIManager.Instance.ShowMiGoBrainReveal(selectedTarget, () =>
                    {
                        GameManager.Instance.EndTurn();
                    });
                });
            }
            else
            {
                Debug.LogWarning("��ǰ������������������ţ��޷�ִ�����Ʋ���");
                GameManager.Instance.EndTurn();
            }
        });
    }
}
