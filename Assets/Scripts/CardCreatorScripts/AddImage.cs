// Name:    CardCollectors
// Date:    11/12/2021
// Description: The AddImage class allows a user to select a portrait for inclusion on the card
// This script is called by the create a card scene when selecting a new image and when a new image image is selected in the Image Selection scene

using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// stackoverflow.com/questions/31765518/how-to-load-an-image-from-url-with-unity

public class AddImage : MonoBehaviour
{
    public InputField cardName;
    public InputField maxCards;
    public InputField description;
    public Toggle summons;
    public InputField life;
    public InputField defense;
    public InputField attack;
    public InputField cost;
    public Dropdown element;
    public InputField spellValue;
    public Dropdown spellType;
    public Image portrait;

    // Once the player clicks an image, set that image to the portrait of the currently selected card
    // After setting the data, return from the select an image scene to the card creator scene
    public void SelectImage(Button clicked)
    {
        // Images are stored as the name of the image in the resources folder
        string imageName = clicked.name;

        CardCollection.newCardData.portrait = imageName;
        SceneManager.LoadScene("CardCreator");
    }

    // This function loads the Image Selection scene so a user can select a new image. 
    // All of the data is stored to the newCardData file so it can be restored when returning to the Card Editor scene
    public void getImage()
    {
        storeInfo();
        CardCollection.newCardInfoExists = true;
        SceneManager.LoadScene("ImageSelecter");

    }

    // Store information so it can be restored when returning from the Image Selector scene to the card editor scene
    private void storeInfo()
    {    
        if (cardName.text != "")
            CardCollection.newCardData.cardName = cardName.text;
        if (maxCards.text != "")
            CardCollection.newCardData.cardMax = int.Parse(maxCards.text);
        if (description.text != "")
            CardCollection.newCardData.description = description.text;
        if (cost.text != "")
            CardCollection.newCardData.cost = int.Parse(cost.text);
        CardCollection.newCardData.summon = summons.isOn;
        if (summons.isOn)
        {
            if (life.text != "")
                CardCollection.newCardData.life = int.Parse(life.text);
            if (defense.text != "")
                CardCollection.newCardData.defense = int.Parse(defense.text);
            if (attack.text != "")
                CardCollection.newCardData.attack = int.Parse(attack.text);
        }
        else
        {
            CardCollection.newCardData.element = (Definitions.ElementSet)element.value;
            CardCollection.newCardData.spellType = spellType.value;
            if (spellValue.text != "")
                CardCollection.newCardData.spellValue = int.Parse(spellValue.text);
        }
        CardCollection.newCardData.portrait = portrait.sprite.name;
    }

}
