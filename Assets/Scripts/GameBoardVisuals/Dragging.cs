using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dragging : MonoBehaviour
{
    CanvasGroup blockEvents;
    public bool dragging = false;
    public bool inGame = false;
    public Transform target = null;
    public Transform startingArea;
    public bool collisionEligible;
    public Vector2 originalPosition;
    private bool targetting;

    // Start is called before the first frame update
    void Start()
    {
        blockEvents = GetComponent<CanvasGroup>();
        if(SceneManager.GetActiveScene().name == "GameBoard")
        {
            inGame = true;
        }
    }

    //  Start dragging a card to attack or play a spell
    public void startDragging()
    {
        startingArea = transform.parent;
        originalPosition = transform.position;
        CardObject obj = getCard(transform);

        // Card is on the table so it can colide
        if (startingArea.name == "EnemySummons" || startingArea.name == "Summons")
        {
            collisionEligible = true;
        }
        // Card is a spell so it can be played from your hand
        else if (startingArea.name == "EnemyHand" || startingArea.name == "PlayerHand")
        {
            collisionEligible = !obj.data.summon;
        }
        // Can't play a summons from your hand to attack
        else
        {
            collisionEligible = false;
        }
        if (obj.draggable)
        {
    
            dragging = true;
            blockEvents.alpha = 0.5f;
            blockEvents.blocksRaycasts = false;
        }
        
    }

    // Detect if there is a collision. If there is a collision resolve teh card
    public void OnTriggerEnter2D(Collider2D coll)
    {
        // Collision detected and its not a player against its own cards
        if (collisionEligible && coll.transform.parent != transform.parent)
        {
            // It's a card. may not need this long term. This also prevents you from attacking something in a players ahnd
            if ( coll.transform.name == "CardDisplay(Clone)")
            {
                CardObject playedCard =getCard(transform);
                CardObject target = getCard(coll.transform);
                Deck player = getPlayer(transform);
                Deck targetPlayer = getTarget(player.playerID);
                // See if the card exists and its target exists. If so execute teh card
                if (playedCard != null && target != null)
                {

                    // Summon attacking
                    if (playedCard.data.summon)
                    {
                        if (coll.transform.parent.name != "EnemyHand" && coll.transform.parent.name != "PlayerHand")
                        {
                            attackSummons(playedCard, target, player, false);
                            cardCleanUp(playedCard);
                        }
                    }

                    // Spells
                    else
                    {
                        playSpell(playedCard, target, player, false);
                        collisionEligible = false;
                        dragging = false;
                    }

                }
            }
        }

    }

    // Clean up card dragging and prevent hte card from being played again this turn
    private void cardCleanUp(CardObject card)
    {
        card.played = true;
        card.draggable = false;
        card.CardDisplay.transform.position = originalPosition;
        collisionEligible = false;
        dragging = false;
    }

    // Play a card to the player area
    public void endDragging()
    {
        dragging = false;               // No longer dragging
        collisionEligible = false;      // Cannot collide again
        blockEvents.alpha = 1.0f;
        blockEvents.blocksRaycasts = true;

        CardObject card = getCard(transform);
        GameObject area;
        Deck player = getPlayer(transform);
        Deck target = getTarget(player.playerID);

        // Play a card from the hand
        if (startingArea.name == "PlayerHand" || startingArea.name == "EnemyHand") 
        { 
            if (player.playerID == 1)
                area = GameObject.Find("PlayerArea/Summons");
            else
                area = GameObject.Find("EnemyArea/EnemySummons");

            // Card is a summon so add it to the table
            if (card.data.summon)
            {
                playSummons(card, area, player);
            }

            // Card is spell so target the correct player
            else
            {
                playSpell(card, target.playerInfo, player, true);
            }

        }

        // Summon is attacking
        else if (!card.played && (startingArea.name == "Summons" || startingArea.name == "EnemySummons"))
        {
            // If there are summons on the board, attack the first one. otherwise attack the player
            CardObject defender;
            bool playerIsTarget = false;

            if (target.table.Count == 0)
            {
                defender = target.playerInfo;
                playerIsTarget = true;
            }
            else
            {
                defender = target.table[0];
            }

            // Exxecute the attack
            if (card != null && target != null)
                attackSummons(card, defender, player, playerIsTarget);

            cardCleanUp(card);
        }
    }

    // Have a summons attack another card or the opposing player
    private void attackSummons(CardObject attacker, CardObject defender, Deck player, bool attackPlayer)
    {
        GameSettings.Instance.engine.useCard(defender, attacker, player);
        checkTarget(defender, getTarget(player.playerID), attackPlayer);
    }

    // Get the player being targeted
    private Deck getTarget(int playerID)
    {
        if (playerID == 1)
            return GameSettings.Instance.player2;
        else
            return GameSettings.Instance.player1;
    }

    // Get which player is moving teh card
    private Deck getPlayer(Transform obj)
    {
        if (obj.parent.name == "Summons")
            return GameSettings.Instance.player1;
        else if (obj.parent.name == "PlayerHand")
            return GameSettings.Instance.player1;
        else if (obj.parent.name == "EnemySummons")
            return GameSettings.Instance.player2;
        else if (obj.parent.name == "EnemyHand")
            return GameSettings.Instance.player2;
        else
            return null;
    }

    // Get which card is being moved
    private CardObject getCard(Transform obj)
    {
        int cardID = obj.GetComponent<ID>().myID;
        if (obj.parent.name == "EnemySummons")
            return GameSettings.Instance.player2.getCardObjectonTable(cardID);
        else if (obj.parent.name == "Summons")
            return GameSettings.Instance.player1.getCardObjectonTable(cardID);
        else if (obj.parent.name == "PlayerHand")
            return GameSettings.Instance.player1.getCardObjectInHand(cardID);
        else if (obj.parent.name == "EnemyHand")
            return GameSettings.Instance.player2.getCardObjectInHand(cardID);
        else
            return null;
        }

    public void onClick()
    {
        return;
        if (inGame)
        {
            if (transform.parent.name == "EnemySummons" || transform.parent.name == "Summons")
            {
                int cardID = transform.GetComponent<ID>().myID;
                Deck player;
                Deck target;
                if (transform.parent.name == "Summons")
                {
                    player = GameSettings.Instance.player1;
                    target = GameSettings.Instance.player2;
                }
                else
                {
                    player = GameSettings.Instance.player2;
                    target = GameSettings.Instance.player1;
                }

                CardObject selected = player.getCardObjectonTable(cardID);
                if (transform.GetComponent<Outline>().enabled)
                {
                    transform.GetComponent<Outline>().enabled = false;
                    GameSettings.Instance.engine.currentAttacker = null;
                } else
                {
                    if(GameSettings.Instance.engine.currentAttacker == null)
                    {
                        transform.GetComponent<Outline>().enabled = true;
                        GameSettings.Instance.engine.currentAttacker = selected;
                    } 
                    else
                    {
                        bool result = GameSettings.Instance.engine.attack(GameSettings.Instance.engine.currentAttacker, selected);
                        if (result)
                        {
                            Debug.Log("I died, remove me");
                        }
                        ID.getByID(GameSettings.Instance.engine.currentAttacker.cardObjectID).transform.GetComponent<Outline>().enabled = false;
                        GameSettings.Instance.engine.currentAttacker = null;
                    }

                    //GameSettings.Instance.engine.attack(attacker, GameSettings.Instance.player2.playerInfo);         
                }
                
            }
                
        }
        
    }

    // Play a summons to the table
    public void playSummons(CardObject currentCard, GameObject area, Deck player)  
    {
        if (currentCard.data.cost <= player.spellPoints && player.table.Count < GameSettings.Instance.maxSummons)
        {
            player.spellPoints -= currentCard.data.cost;
            player.table.Add(currentCard);
            player.hand.Remove(currentCard);
            currentCard.CardDisplay.GetComponent<RectTransform>().SetParent(area.transform, true);
        }

    }

    // Play a spell from a hand
    private void playSpell(CardObject currentCard, CardObject target, Deck player, bool targetIsPlayer)
    {
        if (player.spellPoints > currentCard.data.cost)
        {
            player.spellPoints -= currentCard.data.cost;
            GameSettings.Instance.engine.useCard(target, currentCard, player);
            player.hand.Remove(currentCard);
            Destroy(currentCard.CardDisplay);
            Destroy(currentCard);
            checkTarget(target, getTarget(player.playerID), targetIsPlayer);
        }
    }

    // See if the target should be remvoed from the game and update its life
    private void checkTarget(CardObject target, Deck owner, bool targetIsPlayer)
    {
        Text foundLife = target.CardDisplay.transform.Find("SummonPanel/lblLife").GetComponent<Text>();
        foundLife.text = target.data.life.ToString();

        if (targetIsPlayer || target.data.life > 0)
            return;

            owner.table.Remove(target);
            Destroy(target.CardDisplay);
            Destroy(target);
    }

    // Update is called once per frame
    public void Update()
    {
        if (dragging && inGame)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}
