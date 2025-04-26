using UnityEngine;
using System.Collections.Generic;

public class AiCardEffect1 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        List<Player> targetPlayers = GameManager.Instance.GetAvailableTargets(currentPlayer);

        if (targetPlayers.Count == 0)
        {
            UIManager.Instance.ShowPopup("û��������ҿɹ�ѡ��");
            // ��ȡҪ�����Ŀ�
            Card cardToDiscard = currentPlayer.GetSelectedCard();
            if (cardToDiscard != null)
            {
                currentPlayer.DiscardCard(cardToDiscard);
            }

            GameManager.Instance.EndTurn();
            return;
        }

        // ���� UI ѡ����� + ������
        if (currentPlayer.isHuman == true)
        {
            UIManager.Instance.ShowGuessEffect(targetPlayers, (selectedTarget, guessedNumber) =>
            {

                if (guessedNumber == 1)
                {
                    UIManager.Instance.ShowPopup("���ܲ����� 1��������ѡ��");
                    return;
                }

                int targetValue = selectedTarget.GetHandValue();

                if (targetValue == guessedNumber)
                {
                    UIManager.Instance.Log($"�����ˣ�{selectedTarget.playerName} �������� {targetValue}���������ˣ�");
                    selectedTarget.Eliminate();
                }
                else
                {
                    UIManager.Instance.Log($"�´��ˡ�{selectedTarget.playerName} �������� {targetValue}��������Ϸ");
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
                UIManager.Instance.Log($"�����ˣ�{targetPlayers[randomIndex].playerName} �������� {targetValue}���������ˣ�");
                targetPlayers[randomIndex].Eliminate();
            }
            else
            {
                UIManager.Instance.Log($"�´��ˡ�{targetPlayers[randomIndex].playerName} �������� {targetValue}��������Ϸ");
            }

            GameManager.Instance.EndTurn();
        }
    }
}
