using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // ���ƻ������ݣ�Card ��ʵ������ id�����ơ����͡�value �ȣ�
    private Player owner; // �˿��Ƶĳ�����

    /// <summary>
    /// ���ÿ���������ң��ڿ��Ʒ����������ʱ���ã�
    /// </summary>
    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    /// <summary>
    /// �ⲿ��������������������
    /// </summary>
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("�������ݻ�ӵ����δ���ã�");
            return;
        }

        Debug.Log($"��{owner.playerName}������ˡ�{cardData.cardName}��");

        // ���ݿ������ͷ�������
        if (cardData.isInsane)
        {
            HandleInsaneCard(); // ������߼�
        }
        else
        {
            HandleNormalCard(); // ��ͨ���߼�
        }
    }

    /// <summary>
    /// ������ͨ���ƣ�ʹ�� IMainEffect �ӿڣ�
    /// </summary>
    private void HandleNormalCard()
    {
        var effect = GetComponent<IMainEffect>();
        if (effect != null)
        {
            effect.ExecuteEffect(owner);
        }
        else
        {
            Debug.LogWarning($"��ͨ���� {cardData.cardName} û�й��� IMainEffect Ч���ű���");

        }
    }

    /// <summary>
    /// �������ƣ�ʹ�� IInsaneCard �ӿڣ�
    /// </summary>
    private void HandleInsaneCard()
    {
        var insaneEffect = GetComponent<IInsaneCard>();
        if (insaneEffect == null)
        {
            Debug.LogWarning($"����� {cardData.cardName} û�й��� IInsaneCard Ч���ű���");
            return;
        }

        // ��һ���������״̬��ֻ�ܴ������Ƶ�����Ч��
        if (!owner.IsInsane())
        {
            Debug.Log($"{owner.playerName} ������״̬��ֻ��ʹ������Ч��");
            insaneEffect.ExecuteSaneEffect(owner);
           
        }
        else
        {
            // ����ѷ�񣬿���ѡ��ִ�з�������Ч��
            Debug.Log($"{owner.playerName} �ѷ�񣬵���ѡ�� UI");

            UIInsaneChoice.Instance.Show(
                onSane: () =>
                {
                    Debug.Log("ѡ��ִ�� ����Ч��");
                    insaneEffect.ExecuteSaneEffect(owner);
                    
                },
                onInsane: () =>
                {
                    Debug.Log("ѡ��ִ�� ���Ч��");
                    insaneEffect.ExecuteInsaneEffect(owner);
                    
                }
            );
        }
    }

    /// <summary>
    /// �����һ����ţ��ͼ�����һ�غϣ���̭�Ͳ������ˣ�
    /// </summary>
    private void EndTurnIfNeeded()
    {
        if (owner.IsAlive())
        {
            GameManager.Instance.EndTurn();
        }
    }
}
