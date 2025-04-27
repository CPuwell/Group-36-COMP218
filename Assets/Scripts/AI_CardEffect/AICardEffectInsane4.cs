
using UnityEngine;

public class AICardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�����㽫���»غ�ǰ������������Ч��");

        currentPlayer.SetProtected(true); 
        currentPlayer.GoInsane();

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} cannot be chosen as part of the effects of other players' cards until the start of his next turn.");

        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����㽫�ڱ�����Ϸ����ǰ���ᱻ��̭");

        currentPlayer.SetImmortalThisRound(true);

        UIManager.Instance.ShowPopup($"{currentPlayer.playerName} will not be knocked out until the end of this round.");

        GameManager.Instance.EndTurn();
    }
}

