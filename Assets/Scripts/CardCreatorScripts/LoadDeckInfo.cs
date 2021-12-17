// Name:    CardCollectors
// Date:    11/12/2021
// Description: Loads all current card information for editting. Also restores the card information when an image is selected
// Called when the CardCreator scene is loaded

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDeckInfo : MonoBehaviour
{
    public GameObject SpellPanel;
    public GameObject SummonPanel;
    public Toggle SpellToggle;
    public GameObject error;

    // Start is called before the first frame update
    void Start()
    {
        error.SetActive(false);

        // Scene reloaded after a new image is selected
        if (CardCollection.newCardInfoExists)
        {
            CardCollection.newCardInfoExists = false;
            setGameData(CardCollection.newCardData);
        }

        // CardCreator loaded from Deck Creator scene and a card is being editted
        else if (CardCollection.currentCard > -1)
        {
            setGameData(CardCollection.sortedDeck[CardCollection.currentCard].data);
        }
    }

    // Set all of the card data based on the saveed information
    public void setGameData(CardObjectData data)
    {
        gameObject.transform.Find("CardName").GetComponent<InputField>().text = data.cardName;
        gameObject.transform.Find("MaxCards").GetComponent<InputField>().text = data.cardMax.ToString();
        gameObject.transform.Find("CardDescription").GetComponent<InputField>().text = data.description;
        gameObject.transform.Find("CardCost").GetComponent<InputField>().text = data.cost.ToString();
        Image portrait = gameObject.transform.Find("CardDisplay/imgPortrait").GetComponent<Image>();
        if (data.portrait == "")
            portrait.sprite = Resources.Load<Sprite>("Images/C1");
        else
            portrait.sprite = Resources.Load<Sprite>("Images/" + data.portrait);

        if (data.summon)
        {

            gameObject.transform.Find("SummonEnterPanel/CardLife").GetComponent<InputField>().text = data.life.ToString();
            gameObject.transform.Find("SummonEnterPanel/CardDefense").GetComponent<InputField>().text = data.defense.ToString();
            gameObject.transform.Find("SummonEnterPanel/CardAttack").GetComponent<InputField>().text = data.attack.ToString();

        }
        else
        {
            SpellPanel.SetActive(true);
            SummonPanel.SetActive(false);
            SpellToggle.isOn = true;
            gameObject.transform.Find("SpellEnterPanel/SpellValue").GetComponent<InputField>().text = data.spellValue.ToString();
            gameObject.transform.Find("SpellEnterPanel/SpellEffect").GetComponent<Dropdown>().value = data.spellType;
        }
        Dropdown element = gameObject.transform.Find("CardElement").GetComponent<Dropdown>();
        if (data.element == Definitions.ElementSet.Air)
        {
            element.value = (int)Definitions.ElementSet.Air;
        }
        else if (data.element == Definitions.ElementSet.Earth)
        {
            element.value = (int)Definitions.ElementSet.Earth;
        }
        else if (data.element == Definitions.ElementSet.Fire)
        {
            element.value = (int)Definitions.ElementSet.Fire;
        }
        else if (data.element == Definitions.ElementSet.Water)
        {
            element.value = (int)Definitions.ElementSet.Water;
        }

    }

}
