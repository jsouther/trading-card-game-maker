// Name:    CardCollectors
// Date:    11/12/2021
// Description: This class stores all the information about a deck of cards.
// The class is a singleton so I can share it across multiple scenes. It's used to create decks, create cards, and select images
// Resource: https://stackoverflow.com/questions/57641745/how-can-i-save-the-data-of-some-variables-across-scenes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This class builds a collection of cards based on the contents of the database
// The class makes it easy to manage the various cards being used

public class CardCollection : SingletonClass<CardCollection>
{
    // Sorted list for deck creation
    public static List<CardObject> sortedDeck = new List<CardObject>(); // The deck of cards
    public static CardObjectData newCardData = new CardObjectData();    // A new card that is being created but not part of the deck
    public static int currentCard;                                      // Card being editted 
    public static bool newCardInfoExists;                               // Whether an image is being editted

    // Update the deck display when moving between scenes
    public CardObject updateDisplay(float scale, int index, GameObject canvas)
    {
        sortedDeck[index].createCard(canvas);
        sortedDeck[index].scaleCard(scale);
        return sortedDeck[index];

    }

    // Create a new deck based on data sent to the function
    public void buildDeck(float scale, CardObjectData data, GameObject parent)
    {
        sortedDeck.Add(addCard(scale, data, parent));
    }

    // Add a new card to the deck. Change teh display based on whether it is a summon or spell
    private CardObject addCard(float scale, CardObjectData data, GameObject parent)
    {
        CardObject newCard = parent.AddComponent<CardObject>() as CardObject;

        // Save card data
       newCard.data = data;
       newCard.data.element = newCard.setElement(data.elementString);

        newCard.createCard(parent);
        newCard.scaleCard(scale);
        return newCard;
    }

    //Delete card from deck at index
    public void deleteCard(int index) {
        sortedDeck.RemoveAt(index);
    }
}
