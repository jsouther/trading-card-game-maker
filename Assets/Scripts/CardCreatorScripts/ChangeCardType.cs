// Name:    CardCollectors
// Date:    11/12/2021
// Description: This class changes a card's display from spell to summon and back again
// This is called from the CardCreator scene when the summon/spell toggel button is set

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCardType : MonoBehaviour
{
    public Toggle summonButtonSelected;
    public Image cardTypeDisplay;
    public GameObject SummonEnterPanel;
    public GameObject SummonPanel;
    public GameObject SpellEnterPanel;
    public GameObject SpellPanel;

    // Changes the type of card from a summon to a spell or vice versa
    public void ChangeType()
    {

        bool summonDisplayed;
        if (summonButtonSelected.isOn)
        {
            summonDisplayed = true;
            cardTypeDisplay.sprite = Definitions.summonImage;
        }
        else
        {
            summonDisplayed = false;
            cardTypeDisplay.sprite = Definitions.spellImage;
        }

        // Show which information is active on the card creator scene
        SummonEnterPanel.SetActive(summonDisplayed);
        SummonPanel.SetActive(summonDisplayed);
        SpellEnterPanel.SetActive(!summonDisplayed);
        SpellPanel.SetActive(!summonDisplayed);
    }

    // Start is called before the first frame update. Load the two card types
    void Start()
    {

        ChangeType();
    }

}
