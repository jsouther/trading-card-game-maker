using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public Dropdown gameType;
    public Dropdown customGameType;
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

    public void setOptions()
    {
        switch (gameType.options[gameType.value].text)
        {
            case "Standard":
                startingLife.text = "30";
                spellPointsPerTurn.text = "5";
                startingSpellPoints.text = "5";
                handSize.text = "5";
                maxSize.text = "7";
                cardsPerTurn.text = "1";
                maxSummons.text = "7";
                elementFactor.text = "1";
                timeLimit.text = "20";
                turnLimit.text = "20";
                break;
            case "High Power":
                startingLife.text = "40";
                spellPointsPerTurn.text = "10";
                startingSpellPoints.text = "7";
                handSize.text = "6";
                maxSize.text = "10";
                cardsPerTurn.text = "2";
                maxSummons.text = "10";
                elementFactor.text = "3";
                timeLimit.text = "20";
                turnLimit.text = "20";
                break;
            case "Fast Action":
                startingLife.text = "15";
                spellPointsPerTurn.text = "7";
                startingSpellPoints.text = "10";
                handSize.text = "6";
                maxSize.text = "7";
                cardsPerTurn.text = "2";
                maxSummons.text = "8";
                elementFactor.text = "2";
                timeLimit.text = "10";
                turnLimit.text = "10";
                break;
            case "Slow Growing":
                startingLife.text = "50";
                spellPointsPerTurn.text = "5";
                startingSpellPoints.text = "4";
                handSize.text = "4";
                maxSize.text = "10";
                cardsPerTurn.text = "1";
                maxSummons.text = "10";
                elementFactor.text = "1";
                timeLimit.text = "40";
                turnLimit.text = "40";
                break;
            case "Extended":
                startingLife.text = "50";
                spellPointsPerTurn.text = "3";
                startingSpellPoints.text = "3";
                handSize.text = "4";
                maxSize.text = "5";
                cardsPerTurn.text = "1";
                maxSummons.text = "8";
                elementFactor.text = "1";
                timeLimit.text = "60";
                turnLimit.text = "60";
                break;
        }
    }

    public void setCustomOptions() {
        if (UserManager.allCustomGameData != null){
            UserManager.selectedCustomGameName = customGameType.options[customGameType.value].text;

            foreach (CustomGameData game in UserManager.allCustomGameData) {
                if (UserManager.selectedCustomGameName == game.gameName) {
                    startingLife.text = game.startingLife.ToString();
                    spellPointsPerTurn.text = game.spellPointsPerTurn.ToString();
                    startingSpellPoints.text = game.startingSpellPoints.ToString();
                    handSize.text = game.handSize.ToString();
                    maxSize.text = game.maxSize.ToString();
                    cardsPerTurn.text = game.cardsPerTurn.ToString();
                    maxSummons.text = game.maxSummons.ToString();
                    elementFactor.text = game.maxSummons.ToString();
                    timeLimit.text = game.timeLimit.ToString();
                    turnLimit.text = game.turnLimit.ToString();
                }
            }
        }
    }
    
}

