using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane5 : MonoBehaviour, InsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч����Ŀ�����Ʋ���һ��");

        List<Player> targetPlayers = GameManager.Instance.players.FindAll(
            p => p.IsAlive() && !p.IsProtected()
        );

        if (targets.Count == 0)
        {
            Debug.Log("û����ЧĿ��");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��

        List<Card> cards = target.GetCards();
        if (cards.Count > 0)
        {
            target.DiscardCard(cards[0]);
        }

        target.DrawCard(GameManager.Instance.deck);
        Debug.Log($"{target.playerName} �����Ʋ�������");

        currentPlayer.GoInsane(); // ����Ч��ִ������Ϊinsane
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����͵ȡĿ������ �� ��һ�� �� ǿ�Ƹ���0����");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("û����ЧĿ��");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��

        Card stolenCard = target.RemoveCard();
        if (stolenCard != null)
        {
            currentPlayer.AddCard(stolenCard);
            Debug.Log($"{currentPlayer.playerName} ͵ȡ�� {target.playerName} �����ƣ�{stolenCard.cardName}");

            // ��һ���ƣ���ǰĬ�����ֻ�������ƣ�����������һ�ţ�
            List<Card> myCards = currentPlayer.GetCards();
            if (myCards.Count > 1)
            {
                // TODO��δ������ UI ѡ��Ҫ������
                Card cardToDiscard = myCards[0];
                currentPlayer.DiscardCard(cardToDiscard);
                Debug.Log($"{currentPlayer.playerName} ������ {cardToDiscard.cardName}");

                // ��Ŀ�����ǿ�����0����
                GameManager.Instance.GiveSpecificCardToPlayer(target, "0");
            }
            else
            {
                Debug.Log($"{target.playerName} û�����ƿ�͵��Ч��ʧ��");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
