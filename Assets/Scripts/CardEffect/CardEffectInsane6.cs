using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane6 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч��������ѡ���һ����ҽ�������");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            Debug.Log("û�п��Խ�����Ŀ�����");
            return;
        }

        Player target = targets[0]; // TODO��UIѡ��Ŀ��

        Card myCard = currentPlayer.RemoveCard();
        Card theirCard = target.RemoveCard();

        if (myCard != null && theirCard != null)
        {
            currentPlayer.AddCard(theirCard);
            target.AddCard(myCard);

            Debug.Log($"{currentPlayer.playerName} �� {target.playerName} ����������");
        }
        else
        {
            Debug.Log("����ʧ�ܣ�����һ��û������");
        }

        currentPlayer.GoInsane(); // ����Ч��ִ�к��Ϊinsane
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�������·���������ҵ�����");

        // �����Լ���ɸѡ���л��š�δ���������������Ƶ����
        List<Player> targets = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);
        List<Player> playersWithCards = targets.FindAll(p => p.GetCards().Count > 0);

        if (playersWithCards.Count < 2)
        {
            Debug.Log("û���㹻����Ҳ������·��䣨����2�����ƣ�");
            return;
        }

        // �ռ������������
        List<Card> collectedCards = new List<Card>();
        foreach (Player player in playersWithCards)
        {
            Card card = player.RemoveCard();
            if (card != null)
            {
                collectedCards.Add(card);
                Debug.Log($"�ռ��� {player.playerName} �����ƣ�{card.cardName}");
            }
        }

        // ���������
        Shuffle(collectedCards);

        // �����ԭ��ң�˳����ң�
        for (int i = 0; i < playersWithCards.Count; i++)
        {
            playersWithCards[i].AddCard(collectedCards[i]);
            Debug.Log($"{playersWithCards[i].playerName} ����������ƣ�{collectedCards[i].cardName}");
        }
        GameManager.Instance.EndTurn();
    }

    // Fisher�CYates ϴ���㷨
    private void Shuffle(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Card temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
