using UnityEngine;
using System.Collections.Generic;

public class CardEffect3 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿ���ѡ��");
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

            UIManager.Instance.Log($"{currentPlayer.playerName} �������� {currentValue}");
            UIManager.Instance.Log($"{selectedTarget.playerName} �������� {targetValue}");

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
        })
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
