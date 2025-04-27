using UnityEngine;

public class CardEffectInsane7 : MonoBehaviour, IInsaneCard
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
            Debug.LogError("CardEffectInsane7 �Ҳ��� CardController��");
        }
    }


    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч����7����û������Ч�������ܳ������ƿ��ƣ����ܴ��������һ���ƴ���4��");
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����������һ���ƴ���4����ֱ��Ӯ�ñ���");

        Card otherCard = currentPlayer.GetOtherCard(thisCard);
        if (otherCard == null)
        {
            Debug.Log("û���ҵ���һ�����ƣ�Ч����Ч");
            UIManager.Instance.ShowPopup("�����һ�����޷�ʶ��Ч����Ч");
            GameManager.Instance.EndTurn();
            return;
        }

        Debug.Log($"�����һ�������ǣ�{otherCard.cardName}����ֵ {otherCard.value}��");

        if (otherCard.value > 4)
        {
            Debug.Log($"{currentPlayer.playerName} �ɹ��������7����Ч��������Ӯ�ñ��֣�");
            currentPlayer.WinRound();
            GameManager.Instance.DeclareWinner(currentPlayer);

            // ������ʾ������ʤ��
            UIManager.Instance.ShowPopup("����һ������ֵ���� 4�����Ч����������Ӯ���˱�����Ϸ��");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            Debug.Log("��һ�����Ʋ�����4��Ч����Ч");
            UIManager.Instance.ShowPopup("�����һ�����Ʋ����� 4�����Ч��δ����");
            GameManager.Instance.EndTurn();
        }
    }
}
