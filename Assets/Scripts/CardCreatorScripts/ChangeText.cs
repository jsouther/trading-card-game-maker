// Name:    CardCollectors
// Date:    11/12/2021
// Description: Sets the text on a card as set by the user
// Called from the CardCreator scene whenever a user changes card text
// Pretty simple function that just makes sure the card diplay always matches the user input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public InputField card;
    public Text lbl;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ValueChange()
    {
        if (lbl != null && card != null)
            lbl.text = card.text.ToString();
    }

    // Update is called once per frame
    void Update()         
    {

    }

}
