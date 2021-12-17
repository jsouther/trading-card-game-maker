// Name:    CardCollectors
// Date:    11/12/2021
// Description: Manages changing between scenese. All it does is call the correct schene when needed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour

{

    public void LoadCardCreator()
    {
        CardCollection.currentCard = -1;
        SceneManager.LoadScene("CardCreator");
    }
    public void LoadDeckBuilder()
    {
        UserManager.currentDeckID = -1;
        SceneManager.LoadScene("DeckCreator");
    }
    public void LoadHome()
    {
        UserManager.currentDeckName = null;
        SceneManager.LoadScene("Home");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("GameBoard");
    }

    public void LoadLogin()
    {
        UserManager.userID = null;
        UserManager.username = null;
        SceneManager.LoadScene("Login");
    }
}
