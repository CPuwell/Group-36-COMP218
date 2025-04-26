using System.Collections.Generic;
using UnityEngine;

public static class RuleBasedAI
{
    // ����ڣ�ѡ��һ��������
    public static Card ChooseCard(Player player)
    {
        List<Card> hand = player.GetCards();

        if (hand.Count == 0)
        {
            Debug.LogWarning($"{player.playerName} has no cards to play.");
            return null;
        }

        // ѡһ�źϷ����������
        Card selectedCard = ChooseValidCard(player, hand);

        if (selectedCard != null)
        {
            Debug.Log($"{player.playerName} plays {selectedCard.cardName}");
            return selectedCard;
        }
        else
        {
            Debug.LogWarning($"{player.playerName} no valid card found, playing randomly.");
            return hand[0]; // ����Ҳ����Ϸ��ƣ����һ��
        }
    }

    // �����ȹ���ѡ��Ϸ�����
    private static Card ChooseValidCard(Player player, List<Card> hand)
    {
        hand.Sort((a, b) => a.value.CompareTo(b.value)); // ����ֵ��������

        foreach (var card in hand)
        {
            if (IsValidPlay(player, card))
            {
                return card;
            }
        }

        return null; // û�ҵ��Ϸ���
    }

    // �ж�һ�����Ƿ���Գ�
    private static bool IsValidPlay(Player player, Card card)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(player);
        List<int> handValues = new List<int>();

        foreach (var c in player.GetCards())
        {
            handValues.Add(c.value);
        }

        bool has0 = handValues.Contains(0);
        bool has8 = handValues.Contains(8);

        switch (card.value)
        {
            case 0:
                return false; // ���⣺0�����������
            case 1:
            case 2:
            case 3:
                return true; // ���ơ��Ƚϴ�С���鿴����
            case 5:
                return targets.Count > 0; // �ñ������Ʊ�����Ŀ��
            case 6:
            case 7:
                return true;
            case 8:
                if (player.GetCards().Count > 1)
                    return false; // ��������ʱ��������8
                break;
            default:
                break;
        }

        return true; // ����Ĭ�Ͽ��Գ�
    }
}

