using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform discardZone; // Discard Zone

    public List<Player> players; // Player List
    public Deck deck; // Card Deck
    private int currentPlayerIndex = 0; // Player Index
    //private float turnTime = 60f; // Count Down Time for each turn

    private float ai_turnTime = 2f;
    private float timer; // Timer
    private bool gameEnded = false;
    private bool RoundEnded = false;
    private int playerIndexCounter = 0; // Player Index Counter
    private bool gameStarted = false; // Game Start Flag
    private DeckManager deckManager; // Deck Manager
    public Image deckTopCardImage; // �� Inspector ���ֶ�����


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
        deckManager = FindFirstObjectByType<DeckManager>();

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
        deckManager = FindFirstObjectByType<DeckManager>(); // ȷ���ҵ� DeckManager
        deck = deckManager.logicDeck;
        deck.Shuffle();
        StartCoroutine(LoadUISceneAndStartGame());
       
    }
    private IEnumerator LoadUISceneAndStartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null; // �ȴ���һ֡�������
        }

        // ȷ��UI�����Ѿ������꣬�ټ�������
        foreach (Player player in players)
        {
            player.DrawCard(deck);
            Debug.Log($"Player {player.playerName} has drawn a card.");
        }

        currentPlayerIndex = 0;
        gameStarted = true;
        StartTurn();
    }
    public int GetNextPlayerIndex()
    {
        return playerIndexCounter++;
    }

    //Set Timer
    private void StartTurn()
    {
        
        if (!gameStarted) return; // Avoid starting turn before game starts


        Player currentPlayer = players[currentPlayerIndex];
        currentPlayer.SetProtected(false);
        
        Debug.Log($"[�غϿ�ʼ] {currentPlayer.playerName} ��ǰ����������{currentPlayer.GetCards().Count}");
        if (gameEnded) return; // avoid multiple start turn calls
        if (players[currentPlayerIndex].IsInsane())
        {
            if (players[currentPlayerIndex].RevealAndDiscardTopCards(deck))
            {
                currentPlayer.Eliminate();
            }
            
        }
        
        CheckRoundWinCondition();
        CheckDeck();

        if (!currentPlayer.IsAlive())
        {
            EndTurn(); // �Զ������������
            return;
        }

        

        
        currentPlayer.DrawCard(deck);
        UpdateDeckZoneDisplay();
        if (currentPlayer.isHuman)
        {
            timer = -1f;
        }else
        {
            timer = ai_turnTime;
        }

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
            DeclareWinner(bestPlayer); // ֱ������Ӯ��
        }
        else
        {
            Debug.Log("Game Draw");
            gameEnded = true;
        }

        //Mutipul Round Mode
        //if (winners.Count == 1)
        //{
        //    bestPlayer.WinRound();
        //    CheckGameWinCondition();
        //    EndRound();
        //    if (gameEnded) return;
        //    StartRound();
        //}
        //else
        //{
        //    Debug.Log("Game Draw");
        //}
    }

    //�����ƾ͵��ûغϽ����ķ�����Ȼ����ִ�п���Ч��Ȼ����ʤ������
    //End Turn and Switch to Next Player
    public void EndTurn()
    {
        Debug.Log($"[�غϽ���] {players[currentPlayerIndex].playerName} �ĻغϽ���");
        if (gameEnded) return; // avoid multiple end turn calls
        CheckRoundWinCondition();
        CheckDeck();
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

        if (!players[currentPlayerIndex].isHuman) {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
            Player currentPlayer = players[currentPlayerIndex];         

            Card selectedCard = RuleBasedAI.ChooseCard(currentPlayer);
                if (selectedCard != null)
                {
                    currentPlayer.PlayCard(selectedCard); // �� AI ���������
                }
            

            } 
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
            DeclareWinner(alivePlayers[0]); // ֱ������Ӯ��
        }


        //Multipul Round Mode
        //if (alivePlayers.Count == 1)
        //{
        //    alivePlayers[0].WinRound();

        //    EndRound();
        //    CheckGameWinCondition();
        //    if (gameEnded) return;
        //    StartRound();


        //}
        //else
        //{
        //    return;
        //}

    }

    //public void EndRound()
    //{
    //    RoundEnded = true;
    //    Debug.Log("Round Ended");

    //    // ���������ҵĲ���״̬����񱣻���
    //    foreach (Player player in players)
    //    {
    //        player.SetImmortalThisRound(false);
    //    }
    //}


    //public void StartRound()
    //{
    //    RoundEnded = false;
    //    for (int i = 0; i < players.Count; i++)
    //    {
    //        players[i].Reset();
    //    }
    //    deckManager.InitializeDeck();
    //    deck = deckManager.logicDeck; // �ǳ���Ҫ������ͬ��deck����
    //    deck.Shuffle();

    //    foreach (Player player in players)
    //    {
    //        player.DrawCard(deck);
    //    }
    //    Debug.Log("Round Started");
    //    currentPlayerIndex = 0;
    //    StartTurn();
    //}

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
        gameEnded = true;
        RoundEnded = true;
        Debug.Log($" Game Over! Winner is {winner.playerName} (Player {winner.PlayerIndex})!");
        // ������Լ��ϵ���ʤ��������߷������˵�
    }


    //multipul round mode
    //public void DeclareWinner(Player winner)
    //{
    //    RoundEnded = true;
    //    Debug.Log($"Player {winner.PlayerIndex} Wins!");
    //    // ������������Ϸ���� UI��������
    //}

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
            "�ס���Ĵ�������", "0", null, null,
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
        if (card.cardObject == null)
        {
            Debug.LogWarning($"���õĿ��� {card.cardName} �� cardObject �ǿյģ��������ɣ�");
            // ��������һ���µ� GameObject ��չʾ
        }

        ShowCardInDiscardZone(card);  
    }

    public void ShowCardInDiscardZone(Card card)
    {
        Debug.Log($"Show card in discard zone: {card.cardName}");
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
            button.interactable = false; // �����Button���ͽ���
        }
        else
        {
            // ���ٴ�Error�ˣ���Ϊ����չʾ�õ�Prefab����û��Button
            Debug.Log("���õ���Prefab��û��Button������������á�");
        }

        return;
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

    public void CheckDeck()
    {
        if (deck.IsEmpty())
        {
            Debug.Log("Deck is empty now compare card value");
            CompareCard();
            //EndRound();
        }
    }

    public void UpdateDeckZoneDisplay()
    {
        if (deckTopCardImage == null) return;

        if (deck.IsEmpty())
        {
            deckTopCardImage.gameObject.SetActive(false); // ����
        }
        else
        {
            deckTopCardImage.gameObject.SetActive(true);  // ��ʾ
        }
    }

    public static void RefreshDeckZone()
    {
        if (Instance != null)
        {
            Instance.UpdateDeckZoneDisplay();
        }
    }


}