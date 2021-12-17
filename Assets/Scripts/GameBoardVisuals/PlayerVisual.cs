using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour
{

    public Text lifeTotal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onClick()
    {
        Deck player;
        if (lifeTotal.gameObject.name == "PlayerLifeText")
        {
            player = GameSettings.Instance.player1;
        }
        else
        {
            player = GameSettings.Instance.player2;
        }
        CardObject currentAttacker = GameSettings.Instance.engine.currentAttacker;

        if (currentAttacker != null)
        {
            GameSettings.Instance.engine.attack(currentAttacker, player.playerInfo);
            ID.getByID(GameSettings.Instance.engine.currentAttacker.cardObjectID).transform.GetComponent<Outline>().enabled = false;
            GameSettings.Instance.engine.currentAttacker = null;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(lifeTotal.gameObject.name == "PlayerLifeText")
        {
            lifeTotal.text = string.Format("{0}", GameSettings.Instance.player1.playerInfo.data.life.ToString());
        } else
        {
            lifeTotal.text = string.Format("{0}", GameSettings.Instance.player2.playerInfo.data.life.ToString());
        }
        
    }
}
