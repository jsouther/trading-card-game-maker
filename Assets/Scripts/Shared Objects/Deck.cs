// Name:    CardCollectors
// Date:    11/12/2021
// Description: Deck of cards created to play the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DeckInfo
{
    public int cardID;
    public int deckID;
    public int cardCount;

}

// This class builds a deck of cards as a list. This also shuffles the deck

public class Deck 
{
    public string playerName;
    public int spellPoints;
    public int playerID;

    public DeckInfo deckInfo = new DeckInfo();
    public CardObject playerInfo = new CardObject();
    public List<CardObject> hand = new List<CardObject>();
    public List<CardObject> deck = new List<CardObject>();
    public List<CardObject> table = new List<CardObject>();

    public void SetDeckInfo(DeckInfo deckInfo) {
        this.deckInfo = deckInfo;
    }
    
    public void buildDeck(float scale, CardObjectData data, GameObject parent)
    {
        // Add cards for now. This needs to be fixed so it adds deck cards
        for (int i = 0; i < data.cardCount; i++)
        {
            deck.Add(addCard(scale, data, parent));
        }
    }

    // Add a new card to the deck. The parent determines the object holding the card.
    // The scale factor is how big or small the card is
    private CardObject addCard(float scale, CardObjectData data, GameObject parent)
    {
        CardObject newCard;
        newCard = parent.AddComponent<CardObject>() as CardObject;
        newCard.data = data;
        newCard.data.element = newCard.setElement(data.elementString);
        newCard.createCard(parent);
        newCard.scaleCard(scale);
        newCard.draggable = false;
        return newCard;
    }

    // Get a card in hand by its unique ID
    public CardObject getCardObjectInHand(int ID)
    {
        foreach (CardObject element in hand)
        {
            if (element.cardObjectID == ID)
            {
                return element;
            }
        }

        return null;
    }


    public CardObject getCardObjectonTable(int ID)
    {
        foreach (CardObject element in table)
        {
            if (element.cardObjectID == ID)
            {
                return element;
            }
        }

        return null;
    }

}

// Used to shuffle a deck
// Taken from https://stackoverflow.com/questions/33643104/shuffling-a-stackt
static class ShuffleClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}