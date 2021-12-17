// Name:    CardCollectors
// Date:    11/12/2021
// Description: Stores all the game settings selected by a user when creating a game
// Singleton object so we can pass it between the game creator scene and the actual game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonClass<GameSettings>
{
    public bool created = false;
    public string gameName;             // Name of the game
    public int elementFactor = 2;           // Bonus/penalty for attacking an opposing element
    public int spellPointsTurn = 4;         // Number of spell points gained a turn
    public int startingHandSize = 5;        // Cards in hand at beginning of game
    public int maxHandSize = 7;             // Max cards in hand 
    public int startingLife = 30;             // Starting life
    public int cardsPerTurn = 1;            // Cards drawn from the deck each turn
    public int maxSummons = 4;              // Maximum number of summons in play by 1 player
    public int timeLimit = 15;               // Time before the game ends
    public int turnLimit = 15;               // Max number of turns before the game ends
    public Deck player1 = new Deck();                 // Player 1's information
    public Deck player2 = new Deck();                // Player 2's information
    public string deckName1;
    public string deckName2;
  
    public GameEngine engine = new GameEngine();

}


