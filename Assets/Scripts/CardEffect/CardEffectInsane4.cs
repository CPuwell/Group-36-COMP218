using UnityEngine;

public class CardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�����㽫���»غ�ǰ������������Ч��");
        currentPlayer.SetProtected(true);
        currentPlayer.GoInsane(); // ����Ч�������󣬽���insane״̬
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����㽫�ڱ�����Ϸ����ǰ���ᱻ��̭");
        currentPlayer.SetImmortalThisRound(true);
        // ����Ҫ������� insane����Ϊ���Ч��Ĭ��ֻ���� insane ״̬��ʹ��
        GameManager.Instance.EndTurn();
    }
}
