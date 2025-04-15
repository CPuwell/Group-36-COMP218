using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Player> players; // Player List
    public Deck deck; // Card Deck
    private int currentPlayerIndex = 0; // Player Index
    private float turnTime = 30f; // Count Down Time for each turn
    private float timer; // Timer
    private bool gameEnded = false;
    private bool RoundEnded = false;
    private int playerIndexCounter = 0; // Player Index Counter


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPlayer(string playerName)
    {
        GameObject playerObj = new GameObject(playerName);
        Player newPlayer = playerObj.AddComponent<Player>();

        newPlayer.Initialize(playerIndexCounter, playerName);
        players.Add(newPlayer);
        playerIndexCounter++;

        Debug.Log($"Add player: {playerName}, Index: {newPlayer.PlayerIndex}");
    }

    // Initialize the game
    public void StartGame()
    {
        deck = new Deck();
        deck.Shuffle();

        foreach (Player player in players)
        {
            player.DrawCard(deck);
        }

        currentPlayerIndex = 0;
        StartTurn();
    }

    //Set Timer
    private void StartTurn()
    {
        if (deck.isEmpty)
        {
            Debug.Log("Deck is empty");
            return;
        }

        Player currentPlayer = players[currentPlayerIndex];

        // 在回合开始时取消保护状态
        currentPlayer.SetProtected(false);

        timer = turnTime;
        Debug.Log($"Now, {currentPlayer.playerName} is taking turn");
    }


    public void CompareCard()
    {

        List<Player> alivePlayers = players.FindAll(players => players.IsAlive);
        Player bestPlayer = alivePlayers[0];
        int highestValue = bestPlayer.Hand.GetCardValue();
        List<Player> winners = new List<Player> { bestPlayer };

        for (int i = 0; i < alivePlayers.Count; i++)
        {
            int currentValue = alivePlayers[i].GetHandValue();

            if (currentValue > highestValue)
            {
                highestValue = currentValue;
                bestPlayer = alivePlayers[i];
                winners.clear();
                winners.Add(bestPlayer);
            }
            else if (currentValue == highestValue)
            {
                winners.Add(alivePlayers[i]);
            }
        }

        if (winners.Count == 1)
        {
            bestPlayer.WinRound();
        }
        else
        {
            Debug.Log("Draw");
        }
    }

    //出完牌就调用回合结束的方法，然后先执行卡牌效果然后检查胜利条件
    //End Turn and Switch to Next Player
    public void EndTurn()
    {
        if (gameEnded) return; // avoid multiple end turn calls
        CheckRoundWinCondition();

        if (RoundEnded) return;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        StartTurn();
    }

    //Automatic Play
    public void AutoPlay(int i)
    {
        players[currentPlayerIndex].RandomPlayCard();
    }

    //Timer for each turn
    private void Update()
    {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AutoPlay(i); //If time is up, auto play a card
            EndTurn();
        }
    }

    public void CheckRoundWinCondition()
    {

        if (gameEnded) return; // avoid multiple win condition check


        // If only one player is alive, he wins
        List<Player> alivePlayers = players.FindAll(players => players.IsAlive);
        if (alivePlayers.Count == 1)
        {
            alivePlayers[0].WinRound();

            EndRound();
            CheckGameWinCondition();
            if (gameEnded) return;
            StartRound();
            return;

        }

    }

    public void EndRound()
    {
        RoundEnded = true;
        Debug.Log("Round Ended");

        // 清除所有玩家的不死状态（疯狂保护）
        foreach (Player player in players)
        {
            player.SetImmortalThisRound(false);
        }
    }


    public void StartRound()
    {
        RoundEnded = false;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Reset();
        }
        deck.Shuffle();
        foreach (Player player in players)
        {
            player.DrawCard(deck);
        }
        Debug.Log("Round Started");
    }

    public void CheckGameWinCondition()
    {
        if (RoundEnded == true)
        {
            for (int i = 0; i < players.Count; i++) {
                if (players[i].CheckWin == 2 || players[i].CheckInsaintyWin == 3)
                {
                    DeclareWinner(players[i]);
                }
            }
        }

    }

    private void DeclareWinner(Player winner)
    {
        RoundEnded = true;
        Debug.Log($"Player {winner.PlayerIndex} Wins!");
        // 这里可以添加游戏结束 UI、动画等
    }

    //封装出可以选择的玩家列表
    public List<Player> GetAvailableTargets(Player currentPlayer)
    {
        return players.FindAll(p => p != currentPlayer && p.IsAlive() && !p.IsProtected() && !p.isImmortalThisRound());
    }

    //强制给予某张牌
    public void GiveSpecificCardToPlayer(Player target, string cardId)
    {
        // 你可以根据 cardId 预设构建卡牌（此处为举例）
        Card newCard = new Card(
            "米·戈的大脑容器", "0", null, null, CardType.Special,
            "被强制植入疯狂容器", 0, true // 是疯狂牌
        );
        target.AddCard(newCard);
        Debug.Log($"{target.playerName} 被强制获得卡牌：{newCard.cardName}");
    }

}