using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Adapated a simple timer from:https://answers.unity.com/questions/351420/simple-timer-1.html
// Used to update countdown on gameboard
public class Timer : MonoBehaviour
{

    private float gameTime;
    private int minutes;
    private int seconds;
    public Text gameTimeText;

    // Start is called before the first frame update
    void Start()
    {
        gameTime = GameSettings.Instance.timeLimit * 60;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime -= Time.deltaTime;
        if (gameTime > 0) {
            minutes = (int) gameTime / 60;
            seconds = (int)gameTime % 60;
            if (seconds < 10)
            {
                gameTimeText.text = minutes.ToString() + ":0" + seconds.ToString(); 
            } else
            {
                gameTimeText.text = minutes.ToString() + ":" + seconds.ToString();
            }
        }
    }
}
