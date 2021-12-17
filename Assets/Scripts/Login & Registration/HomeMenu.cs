using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
    //public DeckDropdownData[] allDeckData;
    public Dropdown editDeckDropdown;
    public Dropdown deleteDeckDropdown;
    public Text errorText;
    public Text usernameText;
    public Button playGameBtn;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        errorText.gameObject.SetActive(false);
        usernameText.text = "Welcome, " + UserManager.username + '!';

        UserManager.deckNames.Clear();
        CardCollection.sortedDeck.Clear();

        yield return StartCoroutine(LoadUserDecks());
        playGameBtn.interactable = DoesADeckExist();
    }

    IEnumerator LoadUserDecks() {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/GetDecksInfo.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                FillDeckDropdown(www.downloadHandler.text);
            }
        }
    }

    public void FillDeckDropdown(string json) {
        UserManager.allDeckData = JsonHelper.FromJson<DeckDropdownData>(json); //create deckdropdowndata objects
        
        //Create list of deck names
        foreach (DeckDropdownData deck in UserManager.allDeckData) {
            if (!UserManager.deckNames.Contains(deck.name))
                UserManager.deckNames.Add(deck.name);
        }

        editDeckDropdown.AddOptions(UserManager.deckNames);     //Add deck names as options to dropdown
        deleteDeckDropdown.AddOptions(UserManager.deckNames);
    }

    public void GoToDeckBuilderWithDeckID() {
        string deckname = editDeckDropdown.options[editDeckDropdown.value].text;
        foreach (DeckDropdownData deck in UserManager.allDeckData) {
            if (deck.name == deckname) {
                UserManager.currentDeckID = deck.deckID;
                UserManager.currentDeckName = deckname;
            }
        }

        SceneManager.LoadScene("DeckCreator");
        
    }

    public void StartDeleteDeck() {
        StartCoroutine(CallDeleteDeck());
    }

    IEnumerator CallDeleteDeck() {
        yield return StartCoroutine(DeleteDeck()); //Must wait until the deck is deleted before deletedeck is called
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Reload scene so deck option is removed
    }

    //Get the deckID from the name of the selected item in the input dropdown
    public int getDeckIDFromName (Dropdown dropdown) {
        string deckName = deleteDeckDropdown.options[deleteDeckDropdown.value].text;
        foreach (DeckDropdownData deck in UserManager.allDeckData) {
            if (deck.name == deckName) {
                return deck.deckID;
            }
        }

        return -1;
    }

    //Call webserver to delete selected deck
    IEnumerator DeleteDeck() {
        WWWForm form = new WWWForm();
        form.AddField("deckID", getDeckIDFromName(deleteDeckDropdown));
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/DeleteDeck.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
        }   
    }

    // Check if the list of decknames is empty or not
    public bool DoesADeckExist () {
        if (UserManager.deckNames.Count != 0) {
            errorText.gameObject.SetActive(false);
            return true;
        } else {
            errorText.gameObject.SetActive(true);
            return false;
        }
    }

    public void GoToGameSelect() {
        SceneManager.LoadScene(3);
    }
}