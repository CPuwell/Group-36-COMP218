<<<<<<< HEAD
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class AICardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【正常效果】选择玩家并猜数字（不能猜1），猜中出局");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("wufaxuanze");
            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }
            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
            return;
        }

        // 弹出猜测 UI
        if (currentPlayer.isHuman) { 
            UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
            {
                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                    return;
                }

                int targetValue = selectedTarget.GetHandValue();
                if (targetValue == guessedNumber)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"猜中！{selectedTarget.playerName} 出局！");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}");
                }

                currentPlayer.GoInsane();
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

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        }
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】选一名玩家，若其手牌为1直接出局，否则猜一次");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("wufaxuanze");
            // 获取要弃掉的卡
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowGuessEffect(targets, (selectedTarget, guessedNumber) =>
            {
                int realValue = selectedTarget.GetHandValue();

                if (realValue == 1)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 手牌是 1，直接出局！");
                }
                else
                {
                    if (guessedNumber == 1)
                    {
                        UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                        return;
                    }

                    if (guessedNumber == realValue)
                    {
                        selectedTarget.Eliminate();
                        UIManager.Instance.ShowPopup($"猜中了！{selectedTarget.playerName} 出局！");
                    }
                    else
                    {
                        UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {realValue}");
                    }
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player logic for insane mode
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            int realValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} 选择了 {selectedTarget.playerName}");
            
            if (realValue == 1)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 手牌是 1，直接出局！");
            }
            else
            {
                int[] possibleGuesses = {2, 3, 4, 5, 6, 7, 8}; // Cannot guess 1
                int guessedNumber = possibleGuesses[Random.Range(0, possibleGuesses.Length)];
                
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 猜测数字 {guessedNumber}");
                
                if (guessedNumber == realValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"猜中了！{selectedTarget.playerName} 出局！");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {realValue}");
                }
            }
            
            GameManager.Instance.EndTurn();
        }
    }
=======
using UnityEngine;
using System.Collections.Generic;

public class AICardEffectInsane1 : MonoBehaviour, IInsaneCard
{
    public void ExecuteSaneEffect(Player currentPlayer)
    {
        Debug.Log("【正常效果】选择玩家并猜数字（不能猜1），猜中出局");

        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有其他玩家可以猜测");
            return;
        }

        // 弹出猜测 UI
        UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
        {
            if (guessedNumber == 1)
            {
                UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                return;
            }

            int targetValue = selectedTarget.GetHandValue();
            if (targetValue == guessedNumber)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"猜中！{selectedTarget.playerName} 出局！");
            }
            else
            {
                UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {targetValue}");
            }

            currentPlayer.GoInsane();
            GameManager.Instance.EndTurn();
        });
    }

    public void ExecuteInsaneEffect(Player currentPlayer)
    {
        Debug.Log("【疯狂效果】选一名玩家，若其手牌为1直接出局，否则猜一次");

        List<Player> targets = GameManager.Instance.GetAvailableTargets(currentPlayer);
        if (targets.Count == 0)
        {
            UIManager.Instance.ShowPopup("没有可供选择的玩家");
            return;
        }

        if (currentPlayer.isHuman)
        {
            UIManager.Instance.ShowGuessEffect(targets, (selectedTarget, guessedNumber) =>
            {
                int realValue = selectedTarget.GetHandValue();

                if (realValue == 1)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 手牌是 1，直接出局！");
                }
                else
                {
                    if (guessedNumber == 1)
                    {
                        UIManager.Instance.ShowPopup("不能猜 1，请重新选择");
                        return;
                    }

                    if (guessedNumber == realValue)
                    {
                        selectedTarget.Eliminate();
                        UIManager.Instance.ShowPopup($"猜中了！{selectedTarget.playerName} 出局！");
                    }
                    else
                    {
                        UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {realValue}");
                    }
                }

                GameManager.Instance.EndTurn();
            });
        }
        else
        {
            // AI player logic for insane mode
            Player selectedTarget = targets[Random.Range(0, targets.Count)];
            int realValue = selectedTarget.GetHandValue();

            UIManager.Instance.Log($"AI {currentPlayer.playerName} 选择了 {selectedTarget.playerName}");
            
            if (realValue == 1)
            {
                selectedTarget.Eliminate();
                UIManager.Instance.ShowPopup($"{selectedTarget.playerName} 手牌是 1，直接出局！");
            }
            else
            {
                int[] possibleGuesses = {2, 3, 4, 5, 6, 7, 8}; // Cannot guess 1
                int guessedNumber = possibleGuesses[Random.Range(0, possibleGuesses.Length)];
                
                UIManager.Instance.Log($"AI {currentPlayer.playerName} 猜测数字 {guessedNumber}");
                
                if (guessedNumber == realValue)
                {
                    selectedTarget.Eliminate();
                    UIManager.Instance.ShowPopup($"猜中了！{selectedTarget.playerName} 出局！");
                }
                else
                {
                    UIManager.Instance.ShowPopup($"猜错了。{selectedTarget.playerName} 的手牌是 {realValue}");
                }
            }
            
            GameManager.Instance.EndTurn();
        }
    }
>>>>>>> d2b8dbccd4cca094b84f0b0d7fa966467d65b6b1
}