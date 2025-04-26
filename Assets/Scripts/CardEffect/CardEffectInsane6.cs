using UnityEngine;
using System.Collections.Generic;

public class CardEffectInsane6 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч������Ŀ�꽻������");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("�޷�ѡ�����");

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

        UIManager.Instance.ShowPlayerSelectionSimple(targets, target =>
        {
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

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�������·��������������");

        List<Player> targets = GameManager.Instance.GetAvailableTargetsAllowSelf(currentPlayer);
        List<Player> playersWithCards = targets.FindAll(p => p.GetCards().Count > 0);

        if (playersWithCards.Count < 2)
        {
            UIManager.Instance.ShowPopup("û���㹻����Ҳ������·��䣨����2�����ƣ�");
            
            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            GameManager.Instance.EndTurn();
            return;
        }
        
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

        Shuffle(collectedCards);

        for (int i = 0; i < playersWithCards.Count; i++)
        {
            playersWithCards[i].AddCard(collectedCards[i]);
            Debug.Log($"{playersWithCards[i].playerName} ����������ƣ�{collectedCards[i].cardName}");
        }

        // ʹ�ü򵥵���֪ͨ
        UIManager.Instance.ShowPopup("All the players' hand cards have been redistributed.");

        GameManager.Instance.EndTurn();
    }


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
