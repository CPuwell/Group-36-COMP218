<<<<<<< HEAD
using UnityEngine;

public class AICardEffectInsane8 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�������������ж�ϵͳͳһ�������֣������ڴ��ظ��ж�");
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����������ƶ��з��������...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} ���ƶ������� {insaneDiscards} �ŷ���ƣ��������ʤ��������");
            UIManager.Instance.ShowPopup("�����˷��8���ƣ����ƶ����������Ż����Ϸ���ƣ���Ӯ����������Ϸ��");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                UIManager.Instance.ShowPopup("����ʱ���ڲ���״̬����Ȼ���ʤ������δ���������㲻�ᱻ��̭��");
                GameManager.Instance.EndTurn();
            }
            else
            {
                UIManager.Instance.ShowPopup("�����˷��8���ƣ������ƶ��з���Ʋ������š��㱻��̭��");
                currentPlayer.Eliminate();
            }
        }
    }
}
=======
using UnityEngine;

public class AICardEffectInsane8 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�������������ж�ϵͳͳһ�������֣������ڴ��ظ��ж�");
        currentPlayer.GoInsane();
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч����������ƶ��з��������...");

        int insaneDiscards = currentPlayer.CountInsaneDiscards();

        if (insaneDiscards >= 2)
        {
            Debug.Log($"{currentPlayer.playerName} ���ƶ������� {insaneDiscards} �ŷ���ƣ��������ʤ��������");
            UIManager.Instance.ShowPopup("�����˷��8���ƣ����ƶ����������Ż����Ϸ���ƣ���Ӯ����������Ϸ��");
            GameManager.Instance.DeclareWinner(currentPlayer);
        }
        else
        {
            if (currentPlayer.IsImmortal())
            {
                UIManager.Instance.ShowPopup("����ʱ���ڲ���״̬����Ȼ���ʤ������δ���������㲻�ᱻ��̭��");
                GameManager.Instance.EndTurn();
            }
            else
            {
                UIManager.Instance.ShowPopup("�����˷��8���ƣ������ƶ��з���Ʋ������š��㱻��̭��");
                currentPlayer.Eliminate();
            }
        }
    }
}
>>>>>>> d2b8dbccd4cca094b84f0b0d7fa966467d65b6b1
