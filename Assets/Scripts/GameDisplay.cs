
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class GameDisplay : MonoBehaviour
{
    public Dropdown settings;
    public Dropdown player1DeckOptions;
    public Dropdown player2DeckOptions;
    public Dropdown userGames;
    public GameObject dialogPanel;
    public InputField saveGameOptionsName;

    public InputField spellPointsPerTurn;
    public InputField startingSpellPoints;
    public InputField handSize;
    public InputField maxSize;
    public InputField cardsPerTurn;
    public InputField maxSummons;
    public InputField elementFactor;
    public InputField timeLimit;
    public InputField turnLimit;
    public InputField startingLife;

    // Start is called before the first frame update
    void Start()
    {
        UserManager.customGameNames.Clear();
        StartCoroutine(GetGames());
        settings.value = 1;
        settings.value = 0;
        player1DeckOptions.AddOptions(UserManager.deckNames);     //Add deck names as options to dropdown
        player2DeckOptions.AddOptions(UserManager.deckNames);
        loadCurrentOptions();
    }

    IEnumerator GetGames() {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        
        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/GetGames.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {    //Request sent successfully
                var result = www.downloadHandler.text;
                //Display error
                if (result.Contains("Error:")) {

                } else {    //No error
                    CustomGameData[] gamesData = JsonHelper.FromJson<CustomGameData>(result);
                    if (gamesData.Length > 0)
                    {
                        FillGameDropdown(result);
                    }
                }
            }
        }
    }

    public void FillGameDropdown(string json) {
        UserManager.allCustomGameData = JsonHelper.FromJson<CustomGameData>(json); //create deckdropdowndata objects

        //Create list of game names
        foreach (CustomGameData game in UserManager.allCustomGameData) {
            if (!UserManager.customGameNames.Contains(game.gameName))
                UserManager.customGameNames.Add(game.gameName);
        }

        userGames.AddOptions(UserManager.customGameNames);     //Add deck names as options to dropdown
    }

    public void ShowDialogPanel() {
        dialogPanel.SetActive(true);
    }

    public void CancelSave() {
        dialogPanel.SetActive(false);
    }

    public void CallSaveGameOptions() {
        dialogPanel.SetActive(false);
        setCurrentOptions();
        StartCoroutine(SaveGameOptions());
    }

    IEnumerator SaveGameOptions() {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID);
        form.AddField("gameName", saveGameOptionsName.text);
        form.AddField("spellPointsPerTurn", spellPointsPerTurn.text);
        form.AddField("startingSpellPoints", startingSpellPoints.text);
        form.AddField("handSize", handSize.text);
        form.AddField("maxSize", maxSize.text);
        form.AddField("cardsPerTurn", cardsPerTurn.text);
        form.AddField("maxSummons", maxSummons.text);
        form.AddField("elementFactor", elementFactor.text);
        form.AddField("timeLimit", timeLimit.text);
        form.AddField("turnLimit", turnLimit.text);
        form.AddField("startingLife", startingLife.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/SaveCustomGame.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else { //Request sent successfully
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void CallDeleteGameOptions() {
        setCurrentOptionsNull();
        StartCoroutine(DeleteGameOptions());
    }

    IEnumerator DeleteGameOptions() {
        WWWForm form = new WWWForm();
        var gameName = userGames.options[userGames.value].text;
        var gameID = -1;
        foreach (CustomGameData game in UserManager.allCustomGameData) {
                if (gameName == game.gameName) {
                    gameID = game.gameID;
                }
            }
        form.AddField("gameID", gameID);

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/DeleteCustomGame.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else { //Request sent successfully
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    
    public void setCurrentOptions() {
        UserManager.spellPointsPerTurnValue = spellPointsPerTurn.text;
        UserManager.startingSpellPointsValue = startingSpellPoints.text;
        UserManager.handSizeValue = handSize.text;
        UserManager.maxSizeValue = maxSize.text;
        UserManager.cardsPerTurnValue = cardsPerTurn.text;
        UserManager.maxSummonsValue = maxSummons.text;
        UserManager.elementFactorValue = elementFactor.text;
        UserManager.timeLimitValue = timeLimit.text;
        UserManager.turnLimitValue = turnLimit.text;
        UserManager.startingLifeValue = startingLife.text;
    }

    public void loadCurrentOptions() {
        spellPointsPerTurn.text = UserManager.spellPointsPerTurnValue;
        startingSpellPoints.text = UserManager.startingSpellPointsValue;
        handSize.text = UserManager.handSizeValue;
        maxSize.text = UserManager.maxSizeValue;
        cardsPerTurn.text = UserManager.cardsPerTurnValue;
        maxSummons.text = UserManager.maxSummonsValue;
        elementFactor.text = UserManager.elementFactorValue;
        timeLimit.text = UserManager.timeLimitValue;
        turnLimit.text = UserManager.turnLimitValue;
        startingLife.text = UserManager.startingLifeValue;
    }

    public void setCurrentOptionsNull() {
        UserManager.spellPointsPerTurnValue = null;
        UserManager.startingSpellPointsValue = null;
        UserManager.handSizeValue = null;
        UserManager.maxSizeValue = null;
        UserManager.cardsPerTurnValue = null;
        UserManager.maxSummonsValue = null;
        UserManager.elementFactorValue = null;
        UserManager.timeLimitValue = null;
        UserManager.turnLimitValue = null;
        UserManager.startingLifeValue = null;
    }
}
