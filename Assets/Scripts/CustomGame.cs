using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGameData
{
    public int gameID;
    public string gameName;
    public int spellPointsPerTurn;
    public int startingSpellPoints;
    public int handSize;
    public int maxSize;
    public int cardsPerTurn;
    public int maxSummons;
    public int elementFactor;
    public int timeLimit;
    public int turnLimit;
    public int startingLife;
}

public class CustomGame : MonoBehaviour
{
    public CustomGameData data;

    public void gameSetup (CustomGameData data) 
    {
        this.data = data;
    }
}
