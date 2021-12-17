using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckDropdownData {
    public int deckID;
    public int userID;
    public string name;
}

public class DeckDropdown : MonoBehaviour
{
    public DeckDropdownData data;

    public void deckSetup (DeckDropdownData data) 
    {
        this.data = data;
    }
}
