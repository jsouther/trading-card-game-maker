// Name:    CardCollectors
// Date:    11/12/2021
// Description: The heart of the game. This is the card object that stores all of the information about the card. 
// The CardObjectData is just a data class that stores the card info. 
// The CardObject class is a set of functions used to manipulate the data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// This is the card object that is used during the game
// The card object holds the data for the cards during the game and for the deck creator. 
// Using this limits the number of database requests we need to make
[System.Serializable]
public class CardObjectData {
    public Definitions.ElementSet element;
    public string elementString;
    public string cardName;
    public int cardID = -1;
    public int userID = -1;
    public string description;
    public bool summon = true;
    public int attack = 30;
    public int defense = 0;
    public int life = 15;
    public int cost = 50;
    public int spellValue = 4;
    public int spellType = 0;
    public string portrait;
    public int cardCount = 0;
    public int cardMax = 5;

    public bool canAttack = true;
    //public GameObject CardDisplay;
}

public class CardObject : MonoBehaviour
{
    public CardObjectData data; // The data behhind the card
    public GameObject CardDisplay; // The actual card as displayed on a scene
    public bool played = false;
    public bool draggable = false;
    // Used to associate with the CardDisplay, has the same Id as cardDispaly
    public int cardObjectID;


    // Convert a string about the element to the correct enum value
    public Definitions.ElementSet setElement(string elementString) {
        switch (elementString)
            {
                case "Air":
                    return Definitions.ElementSet.Air;
                case "Fire":
                    return Definitions.ElementSet.Fire;
                case "Earth":
                    return Definitions.ElementSet.Earth;
                case "Water":
                    return Definitions.ElementSet.Water;
            }
        return Definitions.ElementSet.Air; //If incompatible choice, default to Air.
    }
  
    // Create a new card and show it on a scene
    public void createCard(GameObject parent)
    {
        // Create a new CardDisplay object, which is a visual version of the card
        // Put it on the canvase of whatever scene is in view

        Transform top = parent.transform;
        if (CardDisplay == null)
        {
            CardDisplay = Instantiate(Resources.Load("CardDisplay")) as GameObject;
        }
        CardDisplay.transform.SetParent(top);

        // Set the card's name
        Text foundName = CardDisplay.transform.Find("lblName").GetComponent<Text>();
        foundName.text = data.cardName;

        // Set the card description
        Text foundDescription = CardDisplay.transform.Find("lblDescription").GetComponent<Text>();
        foundDescription.text = data.description;

        // Set the card image
        Image portrait = CardDisplay.transform.Find("imgPortrait").GetComponent<Image>();
        if (data.portrait == "")
            portrait.sprite = Resources.Load<Sprite>("Images/C1");
        else
            portrait.sprite = Resources.Load<Sprite>("Images/" + data.portrait);

        // Set the card's element, changing the logo and border as appropriate 
        Image cardImageElement = CardDisplay.transform.Find("imgElement").GetComponent<Image>();
        Image cardImageBorder = CardDisplay.transform.Find("imgFrame").GetComponent<Image>();

        // Set the card cost
        Text foundCost = CardDisplay.transform.Find("lblCost").GetComponent<Text>();
        foundCost.text = data.cost.ToString();

        switch (data.element)
        {
            case Definitions.ElementSet.Air:
                cardImageElement.sprite = Definitions.airLogo;
                cardImageBorder.sprite = Definitions.airBorder;
                break;
            case Definitions.ElementSet.Fire:
                cardImageElement.sprite = Definitions.fireLogo;
                cardImageBorder.sprite = Definitions.fireBorder;
                break;
            case Definitions.ElementSet.Earth:
                cardImageElement.sprite = Definitions.earthLogo;
                cardImageBorder.sprite = Definitions.earthBorder;
                break;
            case Definitions.ElementSet.Water:
                cardImageElement.sprite = Definitions.waterLogo;
                cardImageBorder.sprite = Definitions.waterBorder;
                break;
        }

        // Add ID to card object, used to match a visual cardDisplay to cardObject
        // Then increment ID so it stays unique.
        ID id = CardDisplay.AddComponent<ID>();
        id.myID = GameEngine.id;
        cardObjectID = id.myID;
        GameEngine.id++; 

        // Display spell or summon specific info
        if (data.summon)
            summonDisplay();
        else
            spellDisplay();

    }

    // Hide the spell panel and show the summon info
    private void summonDisplay()
    {
        Image cardImageType = CardDisplay.transform.Find("imgCardType").GetComponent<Image>();
        cardImageType.sprite = Definitions.summonImage;

        // Set the card attack
        Text foundAttack = CardDisplay.transform.Find("SummonPanel/lblAttack").GetComponent<Text>();
        foundAttack.text = data.attack.ToString();

        // Set the card life
        Text foundLife = CardDisplay.transform.Find("SummonPanel/lblLife").GetComponent<Text>();
        foundLife.text = data.life.ToString();

        // Set the card defense
        Text foundDefense = CardDisplay.transform.Find("SummonPanel/lblDefense").GetComponent<Text>();
        foundDefense.text = data.defense.ToString();

        //  Hide the spell panel for summons
        Image panel = CardDisplay.transform.Find("SpellPanel").GetComponent<Image>();
        panel.gameObject.SetActive(false);
    }

    // Hide the summon panel and show the spell info
    private void spellDisplay()
    {

        // Show the spell icon
        Image cardImageType = CardDisplay.transform.Find("imgCardType").GetComponent<Image>();
        cardImageType.sprite = Definitions.spellImage;

        // Set the card's power
        Text foundSpell = CardDisplay.transform.Find("SpellPanel/lblSpell").GetComponent<Text>();
        foundSpell.text = data.spellValue.ToString();

        // Hide the summon panel for spells
        Image panel = CardDisplay.transform.Find("SummonPanel").GetComponent<Image>();
        panel.gameObject.SetActive(false);

    }

    // Scale the image of the card
    // Useful if the scenes need different card sizes
    public void scaleCard(float scale)
    {
        scale = scale * Screen.width / 960;
        float currentX = CardDisplay.transform.localScale.x;
        float currentY = CardDisplay.transform.localScale.y;
        CardDisplay.transform.localScale = new Vector2(currentX * scale / 100, currentY * scale / 100);
    }

}


