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
    public Image deckTopCardImage; // 在 Inspector 中手动拖入


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
        deckManager = FindFirstObjectByType<DeckManager>(); // 确保找到 DeckManager
        deck = deckManager.logicDeck;
        deck.Shuffle();
        StartCoroutine(LoadUISceneAndStartGame());
       
    }
    private IEnumerator LoadUISceneAndStartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null; // 等待下一帧继续检查
        }

        // 确保UI场景已经加载完，再继续发牌
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
        
        Debug.Log($"[回合开始] {currentPlayer.playerName} 当前手牌数量：{currentPlayer.GetCards().Count}");
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
            EndTurn(); // 自动跳过死亡玩家
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
            DeclareWinner(bestPlayer); // 直接宣布赢家
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

    //出完牌就调用回合结束的方法，然后先执行卡牌效果然后检查胜利条件
    //End Turn and Switch to Next Player
    public void EndTurn()
    {
        Debug.Log($"[回合结束] {players[currentPlayerIndex].playerName} 的回合结束");
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
                    currentPlayer.PlayCard(selectedCard); // 让 AI 打出这张牌
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
            DeclareWinner(alivePlayers[0]); // 直接宣布赢家
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

    //    // 清除所有玩家的不死状态（疯狂保护）
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
    //    deck = deckManager.logicDeck; // 非常重要，重新同步deck引用
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
        // 这里可以加上弹出胜利界面或者返回主菜单
    }


    //multipul round mode
    //public void DeclareWinner(Player winner)
    //{
    //    RoundEnded = true;
    //    Debug.Log($"Player {winner.PlayerIndex} Wins!");
    //    // 这里可以添加游戏结束 UI、动画等
    //}

    //封装出可以选择的玩家列表
    public List<Player> GetAvailableTargets(Player currentPlayer)
    {
        return players.FindAll(p => p != currentPlayer && p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }

    public List<Player> GetAvailableTargetsAllowSelf(Player requester)
    {
        return players.FindAll(p => p.IsAlive() && !p.IsProtected() && !p.IsImmortal());
    }


    //强制给予某张牌
    public void GiveSpecificCardToPlayer(Player target, string cardId)
    {
        // 你可以根据 cardId 预设构建卡牌（此处为举例）
        Card newCard = new Card(
            "米·戈的大脑容器", "0", null, null,
            "被强制植入疯狂容器", 0, true // 是疯狂牌
        );
        target.AddCard(newCard);
        Debug.Log($"{target.playerName} 被强制获得卡牌：{newCard.cardName}");
    }

    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void PlayCard(Card card)
    {
        // 找到当前玩家
        Player currentPlayer = players[currentPlayerIndex];
        // 当前玩家打出这张牌
        currentPlayer.PlayCard(card);
        if (card.cardObject == null)
        {
            Debug.LogWarning($"弃置的卡牌 {card.cardName} 的 cardObject 是空的，重新生成！");
            // 重新生成一个新的 GameObject 来展示
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
            cardUI.Flip(true); // 翻面
        }
        else
        {
            Debug.LogError("CardUI component not found on the card prefab.");
        }

        var button = cardObject.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.interactable = false; // 如果有Button，就禁用
        }
        else
        {
            // 不再打Error了，因为弃牌展示用的Prefab可以没有Button
            Debug.Log("弃置的牌Prefab上没有Button组件，跳过禁用。");
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
            deckTopCardImage.gameObject.SetActive(false); // 隐藏
        }
        else
        {
            deckTopCardImage.gameObject.SetActive(true);  // 显示
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