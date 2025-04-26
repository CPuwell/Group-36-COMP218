using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect1 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可供选择");
            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // 调用 UI 选择玩家 + 猜数字
        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
            {

                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("不能猜数字 1，请重新选择");
                    return;
                }

                int targetValue = selectedTarget.GetHandValue();

                if (targetValue == guessedNumber)
                {
                    UIManager.Instance.Log($"猜中了！{selectedTarget.playerName} 的手牌是 {targetValue}，他出局了！");
                    selectedTarget.Eliminate();
                }
                else
                {
                    UIManager.Instance.Log($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}，继续游戏");
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
                int randomIndex = UnityEngine.Random.Range(0, targetPlayers.Count);
  
            int[] options = { 0, 2, 3, 4, 5, 6, 7, 8 };
            int num = options[UnityEngine.Random.Range(0, options.Length)];
            int guessedNumber = num;
            int targetValue = targetPlayers[randomIndex].GetHandValue();

            if (targetValue == guessedNumber)
            {
                UIManager.Instance.Log($"猜中了！{targetPlayers[randomIndex].playerName} 的手牌是 {targetValue}，他出局了！");
                targetPlayers[randomIndex].Eliminate();
            }
            else
            {
                UIManager.Instance.Log($"猜错了。{targetPlayers[randomIndex].playerName} 的手牌是 {targetValue}，继续游戏");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
