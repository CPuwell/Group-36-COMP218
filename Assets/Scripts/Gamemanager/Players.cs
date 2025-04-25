using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Transform[] cardSlots = new Transform[2]; // 牌槽
    public HandUI handUI;
    public int PlayerIndex { get; private set; }  
    public string playerName;
    bool isAlive = true; // Player Status
    public bool isHuman = false; // Player Type
    private int winRounds = 0; // Player Win Round
    private int winRoundsInsane = 0; // Player Win Round Insane
    bool isInsane = false; // Player Insane Status
    bool isProtected = false; // Effect of card 4
    bool isImmortalThisRound = false;// Effect of card insane 4
    private List<Card> discardedCards = new List<Card>();// 弃牌堆

    private Hand hand = new Hand(); // 仍然私有保存

    public Hand Hand => hand;       // 外部通过这个只读属性访问

    public bool IsHuman()
    {
        return isHuman;
    }
    public void DrawCard(Deck deck)
    {
        Card newCard = deck.Draw();
        if (newCard == null)
        {
            Debug.LogWarning($"{playerName} 抽牌失败：deck 空");
            return;
        }

        hand.AddCard(newCard);
        Debug.Log($"{PlayerIndex} 抽到了一张牌：{newCard.cardName}");

        // 设置归属
        CardController controller = newCard.cardObject.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetCardOwner(this);
        }

        //  AI 玩家提早 return，绝不执行 UI
        if (!isHuman)
        {
            Debug.Log($"{playerName} 是 AI，跳过 UI 显示");
            return;
        }

        //  人类玩家才执行以下 UI 操作
        for (int i = 0; i < cardSlots.Length; i++)
        {
            
                Debug.Log($"cardSlot[{i}] = {cardSlots[i]}"); //  打印绑定
               

            if (cardSlots[i] == null)
            {
                Debug.LogError($"[{playerName}] cardSlots[{i}] is null！");
                continue;
            }

            if (cardSlots[i].childCount == 0)
            {
                newCard.cardObject.transform.SetParent(cardSlots[i], false);
                newCard.cardObject.transform.localPosition = Vector3.zero;
                newCard.cardObject.transform.localScale = Vector3.one;

                CardUI cardUI = newCard.cardObject.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.SetCard(newCard, this.hand);
                    cardUI.Flip(false);
                }

                break;
            }
        }


        if (handUI != null)
        {
            Debug.Log($"{playerName} 是人类，更新手牌 UI");


            if (isHuman) { 
                Debug.Log($"{playerName} 是人类，更新手牌 UI");
                handUI.UpdateHandUI(hand.GetCards());
            }
        }
    }





    public void PlayCard(Card card)
    {       
        hand.PlayCard(card);
        Debug.Log($"{playerName} 出牌：{card.cardName}");
        if (handUI != null)
        {
            handUI.UpdateHandUI(hand.GetCards());
        }
    }

    public void WinRound()
    {
        if (isInsane)
        {
            winRoundsInsane++;
            Debug.Log($"{playerName} 赢得了疯狂回合！当前疯狂获胜回合数：{winRoundsInsane}");
        } else
        {
            winRounds++;
            Debug.Log($"{playerName} 赢得了回合！当前理智获胜回合数：{winRounds}");
        }
    }

    public void RandomPlayCard()
    {

        int randomIndex = Random.Range(0, hand.CardCount);
        PlayCard(hand.GetCards()[randomIndex]);
    }


    public void Initialize(int index, string name)
    {
        PlayerIndex = index;
        playerName = name;
    }

    public void Reset()
    {
        hand.ClearHand();
        isAlive = true;
        isInsane = false;
    }

    public int GetHandValue()
    {
        return hand.GetCardValue();
    }

    public int CheckWin()
    {
        return winRounds;
    }

    public int CheckInsaintyWin()
    {
        return winRoundsInsane;
    }


    //以下是zjs加的
    public void Eliminate()
    {
        if (isImmortalThisRound)
        {
            return;
        }
        Debug.Log($"{playerName} 被淘汰了！");
        isAlive = false;
    }


    public bool IsAlive()
    {
        return isAlive;
    }

    public List<Card> GetCards()
    {
        return hand.GetCards();
    }

    public void SetProtected(bool status)
    {
        isProtected = status;
    }

    public bool IsProtected()
    {
        return isProtected;
    }

    public void DiscardCard(Card card)
    {
        hand.Discard(card);
        RecordDiscard(card);

        // 弃掉8号牌 → 立即出局
        if (!IsInsane() && card.value == 8 && card.isInsane)
        {
            Eliminate();
        }
        else if (card.value == 8 && !card.isInsane)
        {
            Eliminate();
        }
    }

    public Card RemoveCard()
    {
        List<Card> cards = hand.GetCards();
        if (cards.Count > 0)
        {
            Card cardToReturn = cards[0];
            hand.ClearHand(); // 只有一张牌，所以直接清空
            return cardToReturn;
        }
        return null;
    }

    public void AddCard(Card card)
    {
        hand.AddCard(card);
    }

    public bool IsInsane()
    {
        return isInsane;
    }

    public void GoInsane()
    {
        isInsane = true;
    }

    public void SetImmortalThisRound(bool status)
    {
        isImmortalThisRound = status;
    }

    public bool IsImmortal()
    {
        return isImmortalThisRound;
    }

    public void RecordDiscard(Card card)
    {
        if (card != null)
        {
            discardedCards.Add(card);
            Debug.Log($"{playerName} 弃牌记录更新：{card.cardName}");

            // 弃掉8号牌 → 立即出局
            if (!IsImmortal() && !IsInsane() && card.value == 8 && card.isInsane)
            {
                Eliminate();
            }
            else if (!IsImmortal() && card.value == 8 && !card.isInsane)
            {
                Eliminate();
            }

            // 弃掉0号牌 → 立即出局
            if (!IsImmortal() && card.value == 0)
            {
                Eliminate();
            }
        }
    }

    public int CountInsaneDiscards()// 计算弃牌堆疯狂牌数量
    {
        return discardedCards.FindAll(c => c.isInsane).Count;
    }

    public void ClearDiscardHistory()
    {
        discardedCards.Clear();
    }

    public Card GetOtherCard(Card excludedCard)
    {
        List<Card> cards = GetCards();

        foreach (Card card in cards)
        {
            if (card != excludedCard)
            {
                return card;
            }
        }

        return null;
    }

    public bool RevealAndDiscardTopCards(Deck deck)
    {
        List<Card> topCards = deck.PeekTopCards(CountInsaneDiscards());

        bool foundInsane = false;

        foreach (Card card in topCards)
        {
            if (card == null) continue;

            if (card.isInsane)
            {
                foundInsane = true; // 发现疯狂牌
            }

            // 用标准弃牌逻辑，把牌放到弃牌堆
            RecordDiscard(card);
        }

        // 把这些牌正式从牌堆中移除
        deck.RemoveTopCards(topCards.Count);

        return foundInsane;
    }




}
