using UnityEngine;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
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
    private List<Player> alivePlayers;

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

    public void PlayersDrawCard()
    {
        foreach (Player player in players)
        {
            player.DrawCard(deck);
        }
    }

    // Initialize the game
    public void StartGame()
    {
        deck = new Deck();
        deck.Shuffle();

        PlayersDrawCard();

        currentPlayerIndex = 0;
        StartTurn();
    }

    //Set Timer and Solve deck empty case
    private void StartTurn()
    {
        if (deck.isEmpty)
        {
            Debug.Log("Deck is empty");
            return;
        }
        players[currentPlayerIndex].DrawCard(deck);
        timer = turnTime;
        Update();
        Debug.Log($"Now, {players[currentPlayerIndex].name} is taking turn");
    }

    public void GetAlivePlayers()
    {
        alivePlayers = players.FindAll(players => players.IsAlive);
    }

    public void CompareCard()
    {

        GetAlivePlayers();
        Player bestPlayer = alivePlayers[0];
        int highestValue = bestPlayer.Hand.GetCardValue();
        List<Player> winners = new List<Player> { bestPlayer };

        for (int i = 1; i < alivePlayers.Count; i++)
        {
            int currentValue = alivePlayers[i].GetHandValue();

            if (currentValue > highestValue)
            {
                highestValue = currentValue;
                bestPlayer = alivePlayers[i];
                winners.Clear();
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
    public void AutoPlay()
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
        GetAlivePlayers();
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

    }

    public void StartRound()
    {
        RoundEnded = false;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Reset();
        }
        deck.Shuffle();
        PlayersDrawCard();
        Debug.Log("Round Started");
    }

    public void CheckGameWinCondition()
    {
        if (RoundEnded)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].CheckWinRounds == 2 || players[i].CheckInsaintyWinRounds == 3)
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
}