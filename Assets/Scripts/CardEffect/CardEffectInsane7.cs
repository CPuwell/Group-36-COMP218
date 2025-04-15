using UnityEngine;

public class CardEffectInsane7 : MonoBehaviour, IInsaneCard
{
    private Card thisCard; // ��ǰ���ſ���

    private void Awake()
    {
        thisCard = GetComponent<Card>();
    }

    /// <summary>
    /// ����Ч�����㲻�������������ƣ�������һ���� > 4���� Hand.CanPlayCard ���ƣ�
    /// �˴���������־��¼
    /// </summary>
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч����7����û������Ч�������ܳ������ƿ��ƣ����ܴ��������һ���ƴ���4��");
        currentPlayer.GoInsane(); // �������Ȼ������״̬
        GameManager.Instance.EndTurn();
    }

    /// <summary>
    /// ���Ч��������һ���ƴ���4������Ӯ�ñ�����Ϸ
    /// </summary>
    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����������һ���ƴ���4����ֱ��Ӯ�ñ���");

        Card otherCard = currentPlayer.GetOtherCard(thisCard);
        if (otherCard == null)
        {
            Debug.Log("û���ҵ���һ�����ƣ�Ч����Ч");
            GameManager.Instance.EndTurn();
            return;
        }

        Debug.Log($"�����һ�������ǣ�{otherCard.cardName}����ֵ {otherCard.value}��");

        if (otherCard.value > 4)
        {
            Debug.Log($"{currentPlayer.playerName} �ɹ��������7����Ч��������Ӯ�ñ��֣�");
            currentPlayer.winRound(); // �ۼ�ʤ���غ�
            GameManager.Instance.EndRound();
            GameManager.Instance.DeclareWinner(currentPlayer); // ��������ʤ�ߣ�����Խ�����Ϊ public��
        }
        else
        {
            Debug.Log("��һ�����Ʋ�����4��Ч����Ч");
        }
    }
}
