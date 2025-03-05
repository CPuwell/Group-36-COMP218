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
        timer = turnTime;
        Update();
        Debug.Log($"Now, {players[currentPlayerIndex].name} is taking turn");
    }

    //End Turn and Switch to Next Player
    public void EndTurn()
    {
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
        for int i = 0; i < players.Count; i++)
        {
            players[i].isIsane = false; //Reset Insane Status
        }
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
            return;
        }

    }

    public void CheckGameWinCondition()
    {
        if (RoundEnded = true)
            {
            for (int i = 0; i < players.Count; i++) { 
                if (players[i].checkWin == 2 || players[i].checkInsaintyWin == 3 )
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
