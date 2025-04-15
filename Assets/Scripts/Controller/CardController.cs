using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card cardData; // �������ݣ����� cardName��cardId��isInsane �ȣ�
    private Player owner; // �˿������ڵ���ң�����ʱ���룩

    public void SetCardOwner(Player player)
    {
        owner = player;
    }

    // ���ƽӿڣ��� Hand �� UI ���ã�
    public void Play()
    {
        if (cardData == null || owner == null)
        {
            Debug.LogWarning("�������ݻ������δ���ã�");
            return;
        }

        Debug.Log($"��{owner.playerName}������ˡ�{cardData.cardName}��");

        // �� Insane Card������ƣ���
        if (cardData.isInsane)
        {
            var effectScript = GetComponent<IInsaneCard>();
            if (effectScript == null)
            {
                Debug.LogWarning("�����ȱ�� IInsaneCard Ч���ű���");
                return;
            }

            if (!owner.IsInsane())
            {
                // ����״̬��ֻ��ִ������Ч�� �� Ȼ���Ϊ insane
                effectScript.ExecuteSaneEffect(owner);
                owner.GoInsane();
                GameManager.Instance.EndTurn();
            }
            else
            {
                // ���״̬������ѡ�� UI
                UIInsaneChoice.Instance.Show(
                    onSane: () =>
                    {
                        effectScript.ExecuteSaneEffect(owner);
                        GameManager.Instance.EndTurn();
                    },
                    onInsane: () =>
                    {
                        effectScript.ExecuteInsaneEffect(owner);
                        GameManager.Instance.EndTurn();
                    }
                );
            }
        }
        else
        {
            // ��ͨ���ƴ���
            var effect = GetComponent<IMainEffect>();
            if (effect != null)
            {
                effect.ExecuteEffect(owner);
            }
            else
            {
                Debug.Log("��ͨ����û�й���Ч���ű�������ִ��");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
