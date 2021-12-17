// Name:    CardCollectors
// Date:    11/12/2021
// Description: The game engine. The class manages all spells, attacks, and end game conditions.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameEngine : MonoBehaviour
{
    public int playerTurn = 1;
    public int currentTurn = 0;
    private DateTime startTime;
    private DateTime endTime;
    private float scale = 25f;
    public CardObject currentAttacker = null;
    // ID to uniquely identify cardDisplays to their cardObject
    public static int id = 0;
    public bool gameOver = false;
    private int winner;
    

    // Track time for game over condition
    void Start()
    {
        startTime = DateTime.UtcNow;
        endTime = startTime.AddMinutes(GameSettings.Instance.timeLimit);
    }

    // Checks for game over conditions. It doesn't check to see if the player's deck is empty.
    bool GameOver() 
    {
        if (currentTurn > GameSettings.Instance.turnLimit)
            return true;
        else if (System.DateTime.UtcNow > endTime)
        {
            return true;
        }
        else if (GameSettings.Instance.player1.playerInfo.data.life < 1)
        {
            winner = 2;
            return true;
        }
        else if (GameSettings.Instance.player2.playerInfo.data.life < 1)
        {
            winner = 1;
            return true;
        }
        return false;
    }

    // Use one card to attack another. 
    public bool attack(CardObject attacker, CardObject defender) 
    {
        attacker.data.canAttack = false;
        bool weak = false;
        bool strong = false;
        if (defender.data.cardName == "Player1" || defender.data.cardName == "Player2")
        {
            Debug.Log(defender.data.cardName);
        }
        else if (attacker.data.element == Definitions.ElementSet.Air)
        {
            if (defender.data.element == Definitions.ElementSet.Fire)
                weak = true;
            else if (defender.data.element == Definitions.ElementSet.Earth)
                strong = true;
        }
        else if (attacker.data.element == Definitions.ElementSet.Fire)
        {
            if (defender.data.element == Definitions.ElementSet.Water)
                weak = true;
            else if (defender.data.element == Definitions.ElementSet.Air)
                strong = true;
        }
        else if (attacker.data.element == Definitions.ElementSet.Water)
        {
            if (defender.data.element == Definitions.ElementSet.Earth)
                weak = true;
            else if (defender.data.element == Definitions.ElementSet.Fire)
                strong = true;
        }
        else if(attacker.data.element == Definitions.ElementSet.Earth)
        {
            if (defender.data.element == Definitions.ElementSet.Air)
                weak = true;
            else if (defender.data.element == Definitions.ElementSet.Water)
                strong = true;
        }
        int damage = attacker.data.attack - defender.data.defense;
        if (strong)
            damage += GameSettings.Instance.elementFactor;
        else if (weak)
            damage -= GameSettings.Instance.elementFactor;

        if (damage > 0)
            defender.data.life -= damage;

        if (defender.data.life <= 0)
            return true;

        return false;
    }


    // Create a deck of cards
    public IEnumerator createCards(Deck player, GameObject parent)
    {
        string cardsJson = "";

        yield return StartCoroutine(getDeckFromDB(player, s => cardsJson = s));
        CardObjectData[] allCardData = JsonHelper.FromJson<CardObjectData>(cardsJson);

        for (int i = 0; i < allCardData.Length; i++)
        {
            player.buildDeck(scale, allCardData[i], parent);
        }
        player.deck.Shuffle();
    }

    // Access cards in database and returns the cards
    IEnumerator getDeckFromDB(Deck player, System.Action<string> json)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        form.AddField("deckID", player.deckInfo.deckID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/GetDeck.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {    //Request sent successfully
                json(www.downloadHandler.text);
            }
        }
    }

    /*
    Returns true if the summon is playable, false otherwise.
    A summon is playable if it costs less than the available spellpoints, there
    is room on the board, and it is your turn.
    TODO: check if table is full and whose turn when player 2 is implemented
    */
    public bool playCard(CardObject card, Deck player)
    {
        if (card.data.cost > player.spellPoints) {
            return false;
        }
        else
        {
            player.spellPoints -= card.data.cost;
            return true;
        }
    }


    public void FixPlayer(Deck player)
    {
        player.playerInfo.data.defense = 0;
        player.playerInfo.data.attack = 0;
    }

    public bool useCard(CardObject target, CardObject card, Deck castingPlayer)
    {
        if (card.data.summon)
            return attack(card, target);
        else
            return CastSpell(target, card, castingPlayer);
    }

    public bool CastSpell(CardObject targetCard, CardObject spellCard, Deck castingPlayer)
    {
        switch (spellCard.data.spellType) 
        {
            case 0: // Reduce life
                spellCard.data.attack = spellCard.data.spellValue;
                attack(spellCard, targetCard);
                break;
            case 1: // Reduce attack
                targetCard.data.attack -= spellCard.data.spellValue;
                break;
            case 2: // Reduce defense
                targetCard.data.defense -= spellCard.data.spellValue;
                break;
            case 3: // Add Life
                targetCard.data.life += spellCard.data.spellValue;
                break;
            case 4: // Add attack
                targetCard.data.attack += spellCard.data.spellValue;
                break;
            case 5: // Add defense
                targetCard.data.defense += spellCard.data.spellValue;
                break;
            case 6: // Drain Life
                int currentLife = targetCard.data.life;
                spellCard.data.attack = spellCard.data.spellValue;
                attack(spellCard, targetCard);
                castingPlayer.playerInfo.data.life = currentLife - targetCard.data.life;
                break;
            case 7: // Increase Spellpoints
                castingPlayer.spellPoints += spellCard.data.spellValue;
                break;
            case 8: // Decrease Spellpoints
                if (castingPlayer.playerID == 1)
                {
                    GameSettings.Instance.player2.spellPoints -= spellCard.data.spellValue;
                    if (GameSettings.Instance.player2.spellPoints < 0)
                        GameSettings.Instance.player2.spellPoints = 0;
                }
                else { 
                    GameSettings.Instance.player1.spellPoints -= spellCard.data.spellValue;
                    if (GameSettings.Instance.player1.spellPoints < 0)
                        GameSettings.Instance.player1.spellPoints = 0;
                 }
                break;
            case 9: // Drain Spellpoints
                if (castingPlayer.playerID == 1)
                {
                    int spelldrain = GameSettings.Instance.player2.spellPoints - spellCard.data.spellValue;
                    if (spelldrain < 0)
                        spelldrain = GameSettings.Instance.player2.spellPoints;
                    GameSettings.Instance.player2.spellPoints -= spelldrain;
                    GameSettings.Instance.player1.spellPoints += spelldrain;
                }
                else
                {
                    int spelldrain = GameSettings.Instance.player1.spellPoints - spellCard.data.spellValue;
                    if (spelldrain < 0)
                        spelldrain = GameSettings.Instance.player1.spellPoints;
                    GameSettings.Instance.player1.spellPoints -= spelldrain;
                    GameSettings.Instance.player2.spellPoints += spelldrain;

                }
                break;
            case 10: // Draw a card
                break;
        }


        if (targetCard.data.life < 1)
            return true;
        else
            return false;
    }

    // Set the target for the current attack. Return true if the target is valid
    // Returns false if unable to attack
    public bool setTarget()
    {
        return true;
    }
    // Draws top card from the deck.
    public void DrawCard(Deck player)
    {
        if (player.deck.Count > 0)
        {
            player.deck[0].draggable = true;
            player.hand.Add(player.deck[0]);
            player.deck.RemoveAt(0);
        }
    }

    public void EndTurn()
    {
        if (playerTurn == 1)
        {
            if (currentTurn != 0)
            {
                GameSettings.Instance.player2.spellPoints += GameSettings.Instance.spellPointsTurn;
            }
            
            DrawCard(GameSettings.Instance.player2);
            playerTurn = 2;
            Prompt.allPrompts.displayPromptAccess("Enemy Turn!", 2);
            // Ready all units to attack
            foreach (CardObject card in GameSettings.Instance.player2.table)
            {
                card.data.canAttack = true;
            }
            StartCoroutine(AITurn.AITurnEngine.doAITurn());
            
        }
        else
        {
            GameSettings.Instance.player1.spellPoints += GameSettings.Instance.spellPointsTurn;
            DrawCard(GameSettings.Instance.player1);
            playerTurn = 1;
            currentTurn += 1;
            Prompt.allPrompts.displayPromptAccess("Your Turn!", 2);
        }
        resetPlayedStatus(GameSettings.Instance.player1.table);
        resetPlayedStatus(GameSettings.Instance.player2.table);
        resetPlayedStatus(GameSettings.Instance.player1.hand);
        resetPlayedStatus(GameSettings.Instance.player2.hand);
        GameOver();
    }


    private void resetPlayedStatus(List<CardObject> cards)
    {
        foreach (CardObject c in cards)
        {
            c.draggable = true;
            c.played = false;
        }

    }

    void Update()
    {
        if (GameOver() && !gameOver)
        {
            Debug.Log("Game Over");
            if (winner == 1)
            {
                Prompt.allPrompts.displayPromptAccess("You Win!", 5);
            } else if (winner == 2)
            {
                Prompt.allPrompts.displayPromptAccess("You Lose :(", 5);
            } else
            {
                Prompt.allPrompts.displayPromptAccess("Game Over", 5);
            }
            
            gameOver = true;
            StartCoroutine(loadHome());
        }
    }

    IEnumerator loadHome() {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Home");
    }
}
