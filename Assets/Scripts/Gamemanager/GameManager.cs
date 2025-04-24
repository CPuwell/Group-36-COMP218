using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform discardZone; // Discard Zone

    public List<Player> players; // Player List
    public Deck deck; // Card Deck
    private int currentPlayerIndex = 0; // Player Index
    private float turnTime = 30f; // Count Down Time for each turn
    private float timer; // Timer
    private bool gameEnded = false;
    private bool RoundEnded = false;
    private int playerIndexCounter = 0; // Player Index Counter
    private bool gameStarted = false; // Game Start Flag


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
        deck = FindFirstObjectByType<DeckManager>().logicDeck;
        deck.Shuffle();

        foreach (Player player in players)
        {
            player.DrawCard(deck);
            Debug.Log($"Player {player.playerName} has drawn a card.");
        }

        currentPlayerIndex = 0;
        gameStarted = true;
        StartTurn();
    }

    //Set Timer
    private void StartTurn()
    {

        if (deck.IsEmpty())
        {
            Debug.Log("Deck is empty");
            return;
        }

        Player currentPlayer = players[currentPlayerIndex];

        Debug.Log($"[�غϿ�ʼ] {currentPlayer.playerName} ��ǰ����������{currentPlayer.GetCards().Count}");

        if (!currentPlayer.IsAlive())
        {
            EndTurn(); // �Զ������������
            return;
        }

        

        currentPlayer.SetProtected(false);
        currentPlayer.DrawCard(deck);
        timer = turnTime;
        Debug.Log($"Now, {currentPlayer.playerName} is taking turn");
    }



    public void CompareCard()
    {

        List<Player> alivePlayers = players.FindAll(players => players.IsAlive());
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

    //�����ƾ͵��ûغϽ����ķ�����Ȼ����ִ�п���Ч��Ȼ����ʤ������
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
        if (!gameStarted || gameEnded) return; // Avoid running the timer before the game starts

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AutoPlay(); //If time is up, auto play a card
            EndTurn();
        }
    }

    public void CheckRoundWinCondition()
    {

        if (gameEnded) return; // avoid multiple win condition check


        // If only one player is alive, he wins
        List<Player> alivePlayers = players.FindAll(players => players.IsAlive());
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

        // ���������ҵĲ���״̬����񱣻���
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
                if (players[i].CheckWin() == 2 || players[i].CheckInsaintyWin() == 3)
                {
                    DeclareWinner(players[i]);
                }
            }
        }

    }

    public void DeclareWinner(Player winner)
    {
        RoundEnded = true;
        Debug.Log($"Player {winner.PlayerIndex} Wins!");
        // ������������Ϸ���� UI��������
    }

    //��װ������ѡ�������б�
    public List<Player> GetAvailableTargets(Player currentPlayer)
    {
        return players.FindAll(p => p != currentPlayer && p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }

    public List<Player> GetAvailableTargetsAllowSelf(Player requester)
    {
        return players.FindAll(p => p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }


    //ǿ�Ƹ���ĳ����
    public void GiveSpecificCardToPlayer(Player target, string cardId)
    {
        // ����Ը��� cardId Ԥ�蹹�����ƣ��˴�Ϊ������
        Card newCard = new Card(
            "�ס���Ĵ�������", "0", null, null, CardType.Special,
            "��ǿ��ֲ��������", 0, true // �Ƿ����
        );
        target.AddCard(newCard);
        Debug.Log($"{target.playerName} ��ǿ�ƻ�ÿ��ƣ�{newCard.cardName}");
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void PlayCard(Card card)
    {
        // �ҵ���ǰ���
        Player currentPlayer = players[currentPlayerIndex];
        // ��ǰ��Ҵ��������
        currentPlayer.PlayCard(card);

        ShowCardInDiscardZone(card);  
        EndTurn();
    }

    public void ShowCardInDiscardZone(Card card)
    {
        foreach (Transform child in discardZone)
        {
            Destroy(child.gameObject);
        }

        GameObject cardObject = Instantiate(card.cardObject, discardZone);
        cardObject.transform.localPosition = Vector3.zero;
        cardObject.transform.localScale = Vector3.one;

        CardUI cardUI = cardObject.GetComponent<CardUI>();
        if (cardUI != null)
        {
            cardUI.Flip(true); // ����
        }
        else
        {
            Debug.LogError("CardUI component not found on the card prefab.");
        }

        var button = cardObject.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.interactable = false; // ���ð�ť
        }
        else
        {
            Debug.LogError("Button component not found on the card prefab.");
        }
    }

    public void ResetGame()
    {
        gameEnded = true;
        gameStarted = false;
        currentPlayerIndex = 0;
        playerIndexCounter = 0;
        RoundEnded = false;
        foreach (Player player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();
    }

}