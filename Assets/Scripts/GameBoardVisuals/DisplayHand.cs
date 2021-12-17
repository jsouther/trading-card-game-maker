using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: Empty deck check when drawing

public class DisplayHand : MonoBehaviour
{

    public GameObject playerHand;
    public GameObject enemyHand;
    public GameObject firstSlot;
    public GameObject lastSlot;
    private GameEngine engine;
    public float scale = 33.0f;

    // Start is called before the first frame update
    // First and last spot must be defined
    public IEnumerator Start()
    {

        if (GameSettings.Instance.player1.playerInfo == null)
        {
            GameSettings.Instance.player1.playerInfo = gameObject.AddComponent(typeof(CardObject)) as CardObject;
            GameSettings.Instance.player2.playerInfo = gameObject.AddComponent(typeof(CardObject)) as CardObject;
            GameSettings.Instance.player1.playerInfo.data = new CardObjectData();
            GameSettings.Instance.player2.playerInfo.data = new CardObjectData();
            GameSettings.Instance.player1.playerInfo.data.life = GameSettings.Instance.startingLife;
            GameSettings.Instance.player2.playerInfo.data.life = GameSettings.Instance.startingLife;
            GameSettings.Instance.player1.playerInfo.data.cardName = "Player 1";           // Player 1's deck of CardObjects
            GameSettings.Instance.player2.playerInfo.data.cardName = "Player 2";           // Player 1's deck of CardObjects
            GameSettings.Instance.engine = gameObject.AddComponent(typeof(GameEngine)) as GameEngine;
        }

        if (!GameSettings.Instance.created)
        {
            GameSettings.Instance.player1.spellPoints = 5;
            GameSettings.Instance.player2.spellPoints = 5;
            GameSettings.Instance.created = true;
            GameSettings.Instance.gameName = "Demo Game";
            GameSettings.Instance.elementFactor = 1;
            GameSettings.Instance.spellPointsTurn = 5;         // Number of spell points gained a turn
            GameSettings.Instance.startingHandSize = 5;        // Cards in hand at beginning of game
            GameSettings.Instance.cardsPerTurn = 1;            // Cards drawn from the deck each turn
            GameSettings.Instance.maxSummons = 6;              // Maximum number of summons in play by 1 player
            GameSettings.Instance.timeLimit = 10;               // Time before the game ends
            GameSettings.Instance.turnLimit = 15;               // Max number of turns before the game ends
            GameSettings.Instance.player1.playerID = 1;
            GameSettings.Instance.player2.playerID = 2;
            GameSettings.Instance.player1.spellPoints = 7;
            GameSettings.Instance.player2.spellPoints = 7;
            GameSettings.Instance.player1.playerInfo.data.life = 25;
            GameSettings.Instance.player2.playerInfo.data.life = 23;
        }

        firstSlot = GameObject.Find("FirstSlot");
        lastSlot = GameObject.Find("LastSlot");
        yield return StartCoroutine(GameSettings.Instance.engine.createCards(GameSettings.Instance.player1, playerHand));
        yield return StartCoroutine(GameSettings.Instance.engine.createCards(GameSettings.Instance.player2, enemyHand));
        GameSettings.Instance.player1.deck.Shuffle();
        GameSettings.Instance.player2.deck.Shuffle();
        DrawStartingHand();
    }

    public void DrawStartingHand()
    {
        if (GameSettings.Instance.startingHandSize == 0)
            GameSettings.Instance.startingHandSize = 5;
        for (int i = 0; i < GameSettings.Instance.startingHandSize; i++)
        {
            GameSettings.Instance.engine.DrawCard(GameSettings.Instance.player1);
            GameSettings.Instance.engine.DrawCard(GameSettings.Instance.player2);
        }
        SpaceAndDisplayHand();
    }




    // Update spacing so cards are equidistant in the hand
    public void SpaceAndDisplayHand()
    {
        FormatHand(GameSettings.Instance.player1);
        FormatHand(GameSettings.Instance.player2);
    }

    private void FormatHand(Deck player)
    {
        Vector3 firstSlotPos = firstSlot.transform.position;
        Vector3 lastSlotPos = lastSlot.transform.position;


        float xSpacing;
        Transform parentObject;
        if (player.playerID == 1) {
            xSpacing = playerHand.GetComponent<RectTransform>().rect.width / (float)(player.hand.Count + 1);
            parentObject = playerHand.transform;
        }
        else
        {
            xSpacing = enemyHand.GetComponent<RectTransform>().rect.width / (float)(player.hand.Count + 1);
            parentObject = enemyHand.transform;
        }
        RectTransform rect = parentObject.GetComponent<RectTransform>();
        for (int i = 0; i < player.hand.Count; i++)
        {
            if (player.hand[i] != null) {
                player.hand[i].CardDisplay.transform.SetParent(parentObject, false);
                player.hand[i].CardDisplay.transform.localPosition = new Vector2(175 - rect.rect.width + xSpacing * i, 0); // rect.rect.width/2 -50 + (xSpacing *i),  rect.rect.height/2); //50+(xSpacing * i), ySpacing); 
            }
        }
    }


    IEnumerator getCardsFromDB(System.Action<string> json)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", UserManager.userID); //TODO: SENDING USERID DOES NOTHING YET

        using (UnityWebRequest www = UnityWebRequest.Post("https://southeja-unity-test.000webhostapp.com/GetCards.php", form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {    //Request sent successfully
                json(www.downloadHandler.text);
            }
        }
    }
    // Update is called once per frame

    void Update()
    {
        FormatHand(GameSettings.Instance.player1);
        FormatHand(GameSettings.Instance.player2);
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameSettings.Instance.engine.DrawCard(GameSettings.Instance.player1);
        }
    }

}
