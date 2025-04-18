using UnityEngine;

public class CardEffectInsane8 : MonoBehaviour, InsaneCard
{
    private void Awake()
    {
        // ������ thisCard ��ȡ
    }

    // ����Ч��ʲô������д�������߼����� RecordDiscard() ��ͳһ����
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�������������ж�ϵͳͳһ������֣������ڴ��ظ��ж�");
        currentPlayer.GoInsane(); // ���Ǵ�����Խ�����״̬
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����������ƶ��з��������...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} ���ƶ������� {insaneDiscards} �ŷ���ƣ��������ʤ��������");
            GameManager.Instance.DeclareWinner(currentPlayer); // ��������Ϊ public
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                GameManager.Instance.EndTurn();
            }
            else
            {
                currentPlayer.Eliminate();
            }
        }
    }

}
