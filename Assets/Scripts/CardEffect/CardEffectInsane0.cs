using UnityEngine;

public class CardEffectInsane0 : MonoBehaviour, InsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【理智效果】你打出了 0号牌（米·戈的大脑容器），立即出局！");
        GameManager.Instance.EndTurn();
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】你打出了 0号牌（米·戈的大脑容器），立即出局！");
        GameManager.Instance.EndTurn();
    }
}