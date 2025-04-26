using UnityEngine;

public class CardEffectInsane4 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�����㽫���»غ�ǰ������������Ч��");

        currentPlayer.SetProtected(true); // ���ñ���
        currentPlayer.GoInsane();         // ���Ǵ�����Ϊ���

        // ����˵����壬ȷ�Ϻ�����غ�
        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            if (currentPlayer.isHuman == true)
            {
                noticeUI.Show(
                    "You cannot be chosen as part of the effects of other players' cards until the start of your next turn.",
                    () => GameManager.Instance.EndTurn()
                );
            }
            else
            {
                Debug.LogWarning("�Ҳ��� UICardEffectNotice��ֱ�ӽ����غ�");
                GameManager.Instance.EndTurn();
            }
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("�����Ч�����㽫�ڱ�����Ϸ����ǰ���ᱻ��̭");

        currentPlayer.SetImmortalThisRound(true); // ���ò���

        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            if (currentPlayer.isHuman == true)
            {
                noticeUI.Show(
                    "You will not be knocked out until the end of this round.",
                    () => GameManager.Instance.EndTurn()
                );
            }
            else
            {
                Debug.LogWarning("�Ҳ��� UICardEffectNotice��ֱ�ӽ����غ�");
                GameManager.Instance.EndTurn();
            }
        }
    }
}
