// Name:    CardCollectors
// Date:    11/12/2021
// Description: This is the deckbuilder scene code. 
// The class controls how this scene operates, allowing a user to create cards, edit cards, and add cards to a deck

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuilder : MonoBehaviour
{
   
    private int currentPage = 0;
    private int itemsPerPage = 10;
    private float scale = 35.0f;

    public Button saveDeckButton;
    private GameObject[] panels = new GameObject[10];   // The areas where the available cards are displayed
    public InputField deckName; 
    public Text error;
    public GameObject confirmWindow;

    // Load all available cards from the database if this is teh first time loading information 
    // If there is already a deck, then just update the display - this happens when you move from the cardcreator scene back to teh deck builder
    IEnumerator Start()
    {
        deckName.text = UserManager.currentDeckName;

        for (int i = 0; i < 10; i++) 
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("CardCount" + i.ToString());
            panels[i] = objects[0];
        }
        if (CardCollection.sortedDeck.Count == 0)
        {
            yield return StartCoroutine(createCards());
        }
        else
        {
            CardObject updatedCard;
            for (int i = 0; i < CardCollection.sortedDeck.Count; i++)
            {
                updatedCard = CardCollection.Instance.updateDisplay(scale, i, gameObject);
            }
        }
        displayCards();

        Debug.Log("DECK ID: " + UserManager.currentDeckID);
    }

    // show the previous page of cards
    public void previous()
    {
        if (currentPage != 0)
        {
            currentPage -= 1;
            displayCards();
        }
    }

    // show the next page of cards
    public void next()
    {
        if ((currentPage+1) * itemsPerPage < CardCollection.sortedDeck.Count)
        {
            currentPage += 1;
            displayCards();
        }
    }

    // Grab all the cards from the database
    IEnumerator createCards()
    {
        string cardsJson = "";
        yield return StartCoroutine(getCardJSONFromDB(s => cardsJson = s));
        CardObjectData[] allCardData = JsonHelper.FromJson<CardObjectData>(cardsJson);
        for (int i = 0; i < allCardData.Length; i++)
        {
            CardCollection.Instance.buildDeck(scale, allCardData[i], gameObject);
        }
    }

    // Show the appropriate cards on the scene. Hide everything not on the current page. 
    public void displayCards()
    {
        int startRange = currentPage * itemsPerPage;
        int endRange = currentPage * itemsPerPage + itemsPerPage;
        int rows = itemsPerPage / 2;
        int totalWidth = 360;
        int spacing = 180;
        int y1 = 120;
        int y2 = -85;

        // Loop through the deck, hiding cards if they aren't on the page and showing them if they are
        for (int i = 0 ; i < CardCollection.sortedDeck.Count; i++) {

            // Show this card
            if (i >= startRange && i < endRange) {
                int x1 = (i - startRange) * spacing - totalWidth;
                int x2 = ((i - startRange) - rows) * spacing - totalWidth;

                if (i - startRange < rows)
                    CardCollection.sortedDeck[i].CardDisplay.transform.localPosition = new Vector2(x1, y1);
                else
                    CardCollection.sortedDeck[i].CardDisplay.transform.localPosition = new Vector2(x2, y2);
                CardCollection.sortedDeck[i].CardDisplay.SetActive(true);
            }

            // Hide this card
            else
            {
                CardCollection.sortedDeck[i].CardDisplay.SetActive(false);
            }
        }

        UpdateCardCount();
    }

    // For each shown card, update the number of cards in the deck and the maximum
    public void UpdateCardCount()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("CardCount");
        int index = currentPage * itemsPerPage; 
        foreach (GameObject p in panels)
        {
            if (index < CardCollection.sortedDeck.Count)
            {
                Text count = p.transform.Find("Edit/txtNumberofCards").GetComponent<Text>();
                count.text = CardCollection.sortedDeck[index].data.cardCount.ToString() + "/" + CardCollection.sortedDeck[index].data.cardMax.ToString();
                index++;
                p.SetActive(true);
            }
            else
            {
                p.SetActive(false);
            }
        }
        updateTotal();
    }

    // Increase the number a selected card in the deck
    public void IncrementCount(int index)
    {
        int arrayIndex = currentPage * itemsPerPage + index;
        if (CardCollection.sortedDeck[arrayIndex].data.cardCount < CardCollection.sortedDeck[arrayIndex].data.cardMax)
            updateCountDisplay(index, 1);
    }

    // Lower the number a selected card in the deck
    public void DecrementCount(int index)
    {
        int arrayIndex = currentPage * itemsPerPage + index;
        if (CardCollection.sortedDeck[arrayIndex].data.cardCount > 0)
            updateCountDisplay(index, -1);
    }

    // Show the number of cards in a deck for a specific card item
    private void updateCountDisplay(int index, int change)
    {
        int actualIndex = currentPage * itemsPerPage + index;
        CardCollection.sortedDeck[actualIndex].data.cardCount += change;
        Text count = panels[index].transform.Find("Edit/txtNumberofCards").GetComponent<Text>();
        count.text = CardCollection.sortedDeck[actualIndex].data.cardCount.ToString() + "/" + CardCollection.sortedDeck[actualIndex].data.cardMax.ToString();
        updateTotal();
    }

    // Update the total number of cards in a deck
    private void updateTotal()
    {
        int total = 0;
        foreach (CardObject c in CardCollection.sortedDeck) {
            total += c.data.cardCount;
        }
        Text size = GameObject.FindGameObjectsWithTag("DeckSize")[0].GetComponent<Text>();
        size.text = "Card Count: " + total.ToString();

    }

    //Returns JSON of EVERY card currently in the database
    IEnumerator getCardJSONFromDB(System.Action<string> json) {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        form.AddField("deckID", UserManager.currentDeckID);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/GetCards.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                json(www.downloadHandler.text);
            }
        }
    }

    // Load the card creator scene
    public void LoadEditCard(int index)
    {
        CardCollection.currentCard = currentPage * itemsPerPage + index; ;
        SceneManager.LoadScene("CardCreator");
    }

    // Save the deck to the database
    public void CallSaveDeck() {
        bool save = true;
        string deckSizeText = GameObject.FindGameObjectsWithTag("DeckSize")[0].GetComponent<Text>().text;   //Get text from scene for decksize
        int deckSize = int.Parse(deckSizeText.Substring(deckSizeText.LastIndexOf(" ") + 1));    //Get only decksize number from string, convert to int
        
        // Validation that the deck has a name and that there are  cards in the deck
        if (string.IsNullOrWhiteSpace(deckName.text)) {
            save = false;
            error.text = "Enter a deck name.";
        } else if (deckSize < 1) {
            save = false;
            error.text = "Must add cards to the deck.";
        }

        if (save) {
            StartCoroutine(SaveDeck());
        }
        else
            error.gameObject.SetActive(true);
    }


    //Saves the deck name to the database and gets back the new deck's ID then calls SaveCardsToDeck
    IEnumerator SaveDeck() {
        saveDeckButton.interactable = false;

        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        form.AddField("deckID", UserManager.currentDeckID);
        form.AddField("name", deckName.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/SaveDeck.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else { //Request sent successfully
                Debug.Log("New Deck ID is: " + www.downloadHandler.text);
                yield return StartCoroutine(SaveCardsToDeck(www.downloadHandler.text));
                UserManager.currentDeckID = -1;
                UserManager.currentDeckName = null;
                SceneManager.LoadScene("Home");
                saveDeckButton.interactable = true;
            }
        }
    }

    //Posts each card to the database with the deckID it belongs to and the number of cards
    IEnumerator SaveCardsToDeck(string deckID) {
        yield return ClearCardsFromDeck(deckID);
        foreach (CardObject card in CardCollection.sortedDeck) {
            if (card.data.cardCount > 0) {
                yield return SaveCardRequest(deckID, card);
            }
        }
    }

    //Clear the deck of any current cards before saving, so editing doesn't result in duplicates
    IEnumerator ClearCardsFromDeck(string deckID) {
        WWWForm form = new WWWForm();
        form.AddField("deckID", deckID);
        Debug.Log("Clearing cards from deckID: " + deckID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/ClearDeck.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
        }
    }

    // Associate the cards in the deck to the deck in the database
    IEnumerator SaveCardRequest(string deckID, CardObject card) {
        WWWForm form = new WWWForm();
        form.AddField("deckID", deckID);
        form.AddField("cardID", card.data.cardID);
        form.AddField("cardCount", card.data.cardCount);
        Debug.Log("Saving Card. Deck ID: " + deckID + " cardID: " + card.data.cardID + " cardCount: " + card.data.cardCount);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/SaveCardsToDeck.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
        }
    }

    // Clicking button to delete card pops up confirmation window.
    public void ShowConfirmation (int index) {
        confirmWindow.SetActive(true);
        CardCollection.currentCard = currentPage * itemsPerPage + index; //Set current card so game knows which to delete
    }

    // Function to perform card deletion if confirmation is accepted
    public void YesSelected() {
        StartCoroutine(DeleteCardFromDB());
    }

    //Cancel deleting card if confirmation is declined
    public void NoSelected() {
        confirmWindow.SetActive(false);
    }

    // Called on click of card delete button. Removes card from CardCollection and database then reload the scene.
    IEnumerator DeleteCardFromDB() {
        WWWForm form = new WWWForm();
        form.AddField("cardID", CardCollection.sortedDeck[CardCollection.currentCard].data.cardID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/DeleteCard.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else if (www.downloadHandler.text == "Success.") {
                CardCollection.Instance.deleteCard(CardCollection.currentCard);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload scene so card is removed
            }
        }
    }

}