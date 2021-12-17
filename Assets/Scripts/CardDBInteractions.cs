using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardDBInteractions : MonoBehaviour
{
    public InputField cardNameField;
    public InputField descriptionField;
    public Image portait;
    public Toggle summon;
    public Toggle spell;
    public Dropdown elementDropdown;
    public Dropdown spellTypeDropdown;
    public InputField costField;
    public InputField lifeField;
    public InputField attackField;
    public InputField defenseField;
    public InputField cardMaxField;
    public InputField spellValueField;
    public GameObject error;
    private CardCollection cards;


    public void CallSaveCard() {
        bool save = true;
        if (cardNameField.text == "")
            save = false;
        else if (descriptionField.text == "")
            save = false;
        else if (costField.text == "")
            save = false;
        else if (cardMaxField.text == "")
            save = false;

        if (summon.isOn)
        {
            if (lifeField.text == "")
                save = false;
            if (attackField.text == "")
                save = false;
            if (defenseField.text == "")
                save = false;
        }
        else if (spell.isOn)
        {
            if (spellValueField.text == "")
                save = false;
        }

        if (save)
            StartCoroutine(SaveCard());
        else
            error.SetActive(true);
            
    }


    IEnumerator SaveCard() {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        if (CardCollection.currentCard != -1) {
            form.AddField("cardID", CardCollection.sortedDeck[CardCollection.currentCard].data.cardID);
        } else {
            form.AddField("cardID", CardCollection.currentCard);
        }
        form.AddField("cardName", cardNameField.text);
        form.AddField("description", descriptionField.text);
        form.AddField("portrait", portait.GetComponent<Image>().sprite.name);
        form.AddField("elementString", elementDropdown.options[elementDropdown.value].text);
        form.AddField("cost", costField.text);
        form.AddField("cardMax", cardMaxField.text);

        if (summon.isOn) {
            form.AddField("summon", 1);
            form.AddField("life", lifeField.text);
            form.AddField("attack", attackField.text);
            form.AddField("defense", defenseField.text);
        } else if (spell.isOn) {
            form.AddField("summon", 0);
            form.AddField("spellValue", spellValueField.text);
            form.AddField("spellType", spellTypeDropdown.value);
        } else {
            Debug.Log("Error: no card type selected.");
        }

        int newCardID = -1; //If saving a new card, php will return its ID.

        //Send POST request for saving/editing card
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/SaveCard.php", form)) {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                newCardID = int.Parse(www.downloadHandler.text);
            }
        }
        //Debug.Log(CardCollection.currentCard);
        if (CardCollection.currentCard == -1)
        {
            CardObject newCard = new CardObject();
            newCard.data = new CardObjectData();
            newCard.CardDisplay = null;
            CardCollection.sortedDeck.Add(newCard);
            CardCollection.currentCard = CardCollection.sortedDeck.Count - 1;
            Debug.Log(CardCollection.sortedDeck.Count);
            CardCollection.sortedDeck[CardCollection.currentCard].data.cardID = newCardID;
        }
        UpdateCard(CardCollection.sortedDeck[CardCollection.currentCard].data);
        SceneManager.LoadScene("DeckCreator");
    }

    private void UpdateCard(CardObjectData data)
    {
        if (cardNameField.text != "")
            data.cardName = cardNameField.text;
        data.element = (Definitions.ElementSet)elementDropdown.value;
        data.portrait = portait.GetComponent<Image>().sprite.name;
        if (descriptionField.text != "") 
            data.description = descriptionField.text;
        else
            data.description = "";
        
        if (costField.text != "")
            data.cost = int.Parse(costField.text);
        else
            data.cost = 0;

        if (cardMaxField.text != "")
            data.cardMax = int.Parse(cardMaxField.text);
        else
            data.cardMax = 0;

        if (summon.isOn)
        {
            data.summon = true;
            if (lifeField.text != "")
                data.life = int.Parse(lifeField.text);
            else
                data.life = 0;
            if (attackField.text != "")
                data.attack = int.Parse(attackField.text);
            else
                data.attack = 0;
            if (defenseField.text != "")
                data.defense = int.Parse(defenseField.text);
            else
                data.defense = 0;
        }
        else if (spell.isOn)
        {
            data.summon = false;
            data.spellType = spellTypeDropdown.value;
            if (spellValueField.text != "")
                data.spellValue = int.Parse(spellValueField.text);
            else
                data.spellValue = 0;

        }
        else
        {
            Debug.Log("Error: no card type selected.");
        }
    }
}
