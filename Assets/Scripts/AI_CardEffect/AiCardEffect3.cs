using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect3 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("无法选择");
             // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard(); 
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        if(currentPlayer.isHuman = true){
        UIManager.Instance.ShowPlayerSelectionSimple(targets, selectedTarget =>
        {
            int currentValue = currentPlayer.GetHandValue();
            int targetValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"{currentPlayer.playerName} 的手牌是 {currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} 的手牌是 {targetValue}");

            if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} 赢了！{selectedTarget.playerName} 出局！");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 赢了！{currentPlayer.playerName} 出局！");
            }
            else
            {
                UIManager.Instance.ShowPopup("平局！没人出局");
            }

            GameManager.Instance.EndTurn();
        });
    }else{
         if (targets.Count> 0)
            {
                int randomIndex = UnityEngine.Random.Range(0,targets.Count);
                int selectedTarget = targets[randomIndex];
            }
        int currentValue = currentPlayer.GetHandValue();

        if (currentValue > targetValue)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} Ӯ�ˣ�{selectedTarget.playerName} ���֣�");
            }
            else if (currentValue < targetValue)
            {
                currentPlayer.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} Ӯ�ˣ�{currentPlayer.playerName} ���֣�");
            }
            else
            {
                UIManager.Instance.ShowPopup("ƽ�֣�û�˳���");
            }

            GameManager.Instance.EndTurn();

    };
    }
}
