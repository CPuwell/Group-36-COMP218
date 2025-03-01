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
    public void AutoPlay()
    {
        player.RandomPlayCard();
    }

    //Timer for each turn
    private void Update()
    {
        timer -= Time.deltaTime; 
        if (timer <= 0)
        {
            AutoPlay(); //If time is up, auto play a card
            EndTurn(); 
        }
    }
}

//后续要添加，获胜条件判定，每回合结束检查是否获胜，同时还有理智值设定的添加，先写一个insanity的script，然后结合胜利条件。
//GameObject方面，需要有桌子的模型，卡牌的模型，timer，游戏开始界面，出牌界面，游戏结束胜利界面，还有等ai做好，ai继承player类。