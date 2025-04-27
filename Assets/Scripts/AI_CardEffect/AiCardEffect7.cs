<<<<<<< HEAD
using UnityEngine;

public class AiCardEffect7 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} 打出了7号牌，没有任何效果");
        GameManager.Instance.EndTurn();
    }
}
=======
using UnityEngine;

public class AiCardEffect7 : MonoBehaviour, IMainEffect
{
    public void ExecuteEffect(Player currentPlayer)
    {
        Debug.Log($"{currentPlayer.playerName} 打出了7号牌，没有任何效果");
        GameManager.Instance.EndTurn();
    }
}
>>>>>>> d2b8dbccd4cca094b84f0b0d7fa966467d65b6b1
