using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prompt : MonoBehaviour
{
    public Text messageText;
    public GameObject prompt;
    public static Prompt allPrompts;


    // Start is called before the first frame update
    void Start()
    {
        allPrompts = this;
        StartCoroutine(displayPrompt("Your Turn", 2));
    }

    IEnumerator displayPrompt(string myText, float seconds)
    {
        prompt.SetActive(true);
        messageText.text = myText;
        yield return new WaitForSeconds(seconds);
        prompt.SetActive(false);
    }

    public void displayPromptAccess(string myText, float seconds)
    {
        prompt.SetActive(true);
        StartCoroutine(this.displayPrompt(myText, seconds));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
