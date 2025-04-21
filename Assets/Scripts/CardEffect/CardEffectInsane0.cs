using UnityEngine;

public class CardEffectInsane0 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч���������� 0���ƣ��ס���Ĵ������������������֣�");
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч���������� 0���ƣ��ס���Ĵ������������������֣�");
        GameManager.Instance.EndTurn();
    }
}