using UnityEngine;

public class CardEffect4 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log("������Ч�������ñ�����ֱ���´λغϿ�ʼ�����ܱ��������ѡ��");

        // �������Ϊ protected
        currentPlayer.SetProtected(true);

        // ����˵����壬���ȷ�Ϻ���ִ�лغϽ���
        var noticeUI = Object.FindFirstObjectByType<UICardEffectNotice>();
        if (noticeUI != null)
        {
            noticeUI.Show(
                "You cannot be chosen as part of the effects of other players' cards until the start of your next turn.",
                () =>
                {
                    GameManager.Instance.EndTurn();
                }
            );
        }
        else
        {
            Debug.LogWarning("UICardEffectNotice ���δ�ҵ���ֱ�ӽ����غ�");
            GameManager.Instance.EndTurn();
        }
    }
}
