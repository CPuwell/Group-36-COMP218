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
    private bool turn_ended = false; // Turn Ended Flag
    private float ai_turnTime = 5f;
    
    private float timer; // Timer
   
    private bool gameEnded = false;
    private bool RoundEnded = false;
    private int playerIndexCounter = 0; // Player Index Counter
    private bool gameStarted = false; // Game Start Flag
    private DeckManager deckManager; // Deck Manager
    public Image deckTopCardImage; 


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

        deckManager = FindFirstObjectByType<DeckManager>(); // Find DeckManager
        deck = deckManager.logicDeck;
        deck.Shuffle();
        StartCoroutine(LoadUISceneAndStartGame());
       
    }
    private IEnumerator LoadUISceneAndStartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait for next frame to continue checking
        }

        
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
        turn_ended = false; // Reset turn ended flag    
        if (!gameStarted) return; // Avoid starting turn before game starts

        NoticeController.Instance.SetNotice($"{players[currentPlayerIndex].playerName}'s Turn");

        Player currentPlayer = players[currentPlayerIndex];
        currentPlayer.SetProtected(false);

        if (!currentPlayer.IsHuman())
        {
            UIManager.Instance.ShowAITurnIndicatorByPlayer(currentPlayer);
        }
        else
        {
            UIManager.Instance.ClearAITurnIndicators(); // 如果是玩家，关闭所有圈圈
        }

        if (gameEnded) return;
        if (gameEnded) return; // avoid multiple start turn calls
        if (players[currentPlayerIndex].IsInsane() && currentPlayer.IsAlive())
        {
            if (players[currentPlayerIndex].RevealAndDiscardTopCards(deck))
            {
                currentPlayer.Eliminate();
                Debug.Log($"{currentPlayer.playerName} is eliminated due to insanity.");
                UIManager.Instance.ShowPopup($"{currentPlayer.playerName} is eliminated due to insanity.");
            }
            
        }
        
        CheckRoundWinCondition();
        CheckDeck();
        
        if (!currentPlayer.IsAlive())
        {
            EndTurn(); // Automatically end turn if player is not alive
            return;
        }
        Debug.Log($"[Turn Start] {currentPlayer.playerName} Current hand count:{currentPlayer.GetCards().Count}");



        currentPlayer.DrawCard(deck);
        UpdateDeckZoneDisplay();
        if (currentPlayer.isHuman)
        {
            timer = -1f;
        }else
        {
            timer = ai_turnTime;
        }

        UIManager.Instance.UpdateImmortalIndicators(players);

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
                Debug.Log($"{alivePlayers[i].playerName}'s card value is {currentValue} now highest value is {highestValue}");
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
            DeclareWinner(bestPlayer); // Declare the winner
        }
        else
        {
            Debug.Log("Game Draw");
            gameEnded = true;
            UIManager.Instance.ShowDrawPanel();
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


    //End Turn and Switch to Next Player
    public void EndTurn()
    {

        turn_ended = true; // Set turn ended flag
        Debug.Log($"[Turn End] {players[currentPlayerIndex].playerName}'s turn ended");
        if (gameEnded) return; // avoid multiple end turn calls
        CheckRoundWinCondition();
        CheckDeck();
        if (RoundEnded) return;

        // Update the current player index
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        if (players[currentPlayerIndex].IsAlive())
        {
            StartCoroutine(WaitAndStartNextTurn());
        }
        else
        {          
            StartTurn();
        }
    }

    private IEnumerator WaitAndStartNextTurn()
    {
        Debug.Log("Waiting 5 seconds before next turn...");
        timer = 10f;
        yield return new WaitForSeconds(5f); // Pause for 5 seconds for showing played card

        
        
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
                    currentPlayer.PlayCard(selectedCard); // Ai will play the card
                    if (!turn_ended)
                    {
                        EndTurn(); // End turn after playing
                    }
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
            DeclareWinner(alivePlayers[0]); // Declare the winner
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

    //    
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
    //    deck = deckManager.logicDeck; 
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
                if (players[i].CheckWin() == 2 || players[i].CheckInsanityWin() == 3)
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
        Debug.Log($"Game Over! Winner is {winner.playerName} (Player {winner.PlayerIndex})!");

        UIManager.Instance.ClearAITurnIndicators();

        if (winner.isHuman)  // Plaer win
        {
            UIManager.Instance.ShowVictoryPanel();
        }
        else  // Ai win
        {
            UIManager.Instance.ShowDefeatPanel($"You were defeated by {winner.playerName}!");
        }
    }



    //multipul round mode
    //public void DeclareWinner(Player winner)
    //{
    //    RoundEnded = true;
    //    Debug.Log($"Player {winner.PlayerIndex} Wins!");
    //    
    //}


    public List<Player> GetAvailableTargets(Player currentPlayer)
    {
        return players.FindAll(p => p != currentPlayer && p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }

    public List<Player> GetAvailableTargetsAllowSelf(Player requester)
    {
        return players.FindAll(p => p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }


    //Force-give a specific card
    public void GiveSpecificCardToPlayer(Player target, string cardId)
    {
        Card newCard = new Card(
            "Mi-Go Brain Cylinder", "0", null, null,
            "Forced into madness container", 0, true
        );
        target.AddCard(newCard);
        Debug.Log($"{target.playerName} was forcibly given the card: {newCard.cardName}");
    }


    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void PlayCard(Card card)
    {
        // Find current player
        Player currentPlayer = players[currentPlayerIndex];
        
        currentPlayer.PlayCard(card);
        if (card.cardObject == null)
        {
            Debug.LogWarning($"Discarded card {card.cardName} has a null cardObject, recreating!");
            
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
            cardUI.Flip(true); // Flip
        }
        else
        {
            Debug.LogError("CardUI component not found on the card prefab.");
        }

        var button = cardObject.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.interactable = false; 
        }
        else
        {
            
            Debug.Log("No Button component on discard card prefab. Skipped disabling.");
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
        if (deck.IsEmpty() && gameEnded == false)
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
            deckTopCardImage.gameObject.SetActive(false); // Hide
        }
        else
        {
            deckTopCardImage.gameObject.SetActive(true);  // Display
        }
    }

    public static void RefreshDeckZone()
    {
        if (Instance != null)
        {
            Instance.UpdateDeckZoneDisplay();
        }
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public void TriggerPlayerDefeat(string defeatMessage)
    {
        gameEnded = true;
        RoundEnded = true;

        UIManager.Instance.ShowDefeatPanel(defeatMessage);
    }

}