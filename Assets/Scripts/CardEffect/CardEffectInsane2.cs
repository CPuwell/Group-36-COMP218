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
            Debug.Log("û�пɲ鿴��Ŀ�����");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��

        List<Card> cards = target.GetCards();
        if (cards.Count > 0)
        {
            Debug.Log($"��鿴�� {target.playerName} �����ƣ�{cards[0].cardName}");
        }
        else
        {
            Debug.Log($"{target.playerName} û������");
        }

        currentPlayer.GoInsane(); // ����Ч�������������״̬
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����鿴Ŀ��������� �� �Լ���һ�� �� ��һ��");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("û�пɲ鿴��Ŀ�����");
            return;
        }

        Player target = targets[0]; // TODO: �滻Ϊ UI ѡ��

        List<Card> targetCards = target.GetCards();
        if (targetCards.Count > 0)
        {
            Debug.Log($"��鿴�� {target.playerName} �����ƣ�{targetCards[0].cardName}");
        }
        else
        {
            Debug.Log($"{target.playerName} û������");
        }

        // �Լ���һ����
        currentPlayer.DrawCard(GameManager.Instance.deck);

        // ��һ���ƣ���ǰĬ�����ֻ�������ƣ�����������һ�ţ�
        List<Card> myCards = currentPlayer.GetCards();
        if (myCards.Count > 1)
        {
            // TODO��δ������ UI ѡ��Ҫ������
            Card cardToDiscard = myCards[0];
            currentPlayer.DiscardCard(cardToDiscard);
            Debug.Log($"{currentPlayer.playerName} ������ {cardToDiscard.cardName}");
        }
        else if (myCards.Count == 1)
        {
            // �������������û���ƣ�ֻ����һ�ţ��ǾͲ���
            Debug.Log($"{currentPlayer.playerName} ֻ��һ���ƣ���ִ������");
        }
        GameManager.Instance.EndTurn();
    }
}