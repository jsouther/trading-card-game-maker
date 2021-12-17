// Name:    CardCollectors
// Date:    11/12/2021
// Description: Creates a new game, storing all the information in the game settings singleton object
// Called when creating a game from the GameSelect scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateGame : MonoBehaviour
{
    public InputField gameName;
    public InputField spellPointsPerTurn;
    public InputField startingSpellPoints;
    public InputField handSize;
    public InputField maxSize;
    public InputField cardsPerTurn;
    public InputField maxSummons;
    public InputField elementFactor;
    public InputField timeLimit;
    public InputField turnLimit;
    public InputField startingLife;
    public Dropdown player1dropdown;
    public Dropdown player2dropdown;
    public GameOptions customOptions;

    public void createGame()
    {
        GameSettings.Instance.created = true;
        GameSettings.Instance.gameName = gameName.text;
        GameSettings.Instance.elementFactor = int.Parse(elementFactor.text);
        GameSettings.Instance.spellPointsTurn = int.Parse(spellPointsPerTurn.text);         // Number of spell points gained a turn
        GameSettings.Instance.startingHandSize = int.Parse(handSize.text);        // Cards in hand at beginning of game
        GameSettings.Instance.cardsPerTurn = int.Parse(cardsPerTurn.text);            // Cards drawn from the deck each turn
        GameSettings.Instance.maxSummons = int.Parse(maxSummons.text);              // Maximum number of summons in play by 1 player
        GameSettings.Instance.timeLimit = int.Parse(timeLimit.text);               // Time before the game ends
        GameSettings.Instance.turnLimit = int.Parse(turnLimit.text);               // Max number of turns before the game ends
        GameSettings.Instance.turnLimit = int.Parse(turnLimit.text);               // Max number of turns before the game ends
        GameSettings.Instance.player1 = new Deck();
        GameSettings.Instance.player2 = new Deck();
        GameSettings.Instance.player1.playerID = 1;
        GameSettings.Instance.player2.playerID = 2;
        GameSettings.Instance.player1.spellPoints = int.Parse(startingSpellPoints.text);
        GameSettings.Instance.player2.spellPoints = int.Parse(startingSpellPoints.text);
        GameSettings.Instance.startingLife = int.Parse(startingLife.text);

        GameSettings.Instance.player1.deckInfo.deckID = getDeckIDFromName(player1dropdown);
        GameSettings.Instance.player2.deckInfo.deckID = getDeckIDFromName(player2dropdown);

        SceneManager.LoadScene("GameBoard");
    }

    public int getDeckIDFromName (Dropdown playerDropdown) {
        string deckName = playerDropdown.options[playerDropdown.value].text;
        foreach (DeckDropdownData deck in UserManager.allDeckData) {
            if (deck.name == deckName) {
                return deck.deckID;
            }
        }

        return -1;
    }

}
