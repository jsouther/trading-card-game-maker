using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandtoTable : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        return;

    }


    private Deck getPlayer(Transform obj)
    {
        if (obj.parent.name == "Summons")
            return GameSettings.Instance.player2;
        else if (obj.parent.name == "PlayerHand")
            return GameSettings.Instance.player1;
        else if (obj.parent.name == "EnemySummons")
            return GameSettings.Instance.player2;
        else if (obj.parent.name == "EnemyHand")
            return GameSettings.Instance.player2;
        else
            return null;
    }

    private CardObject getCard(Transform obj)
    {
        int cardID = obj.GetComponent<ID>().myID;
        if (obj.parent.name == "EnemySummons")
            return GameSettings.Instance.player2.getCardObjectonTable(cardID);
        else if (obj.parent.name == "Summons")
            return GameSettings.Instance.player1.getCardObjectonTable(cardID);
        else if (obj.parent.name == "PlayerHand")
            return GameSettings.Instance.player1.getCardObjectInHand(cardID);
        else if (obj.parent.name == "EnemyHand")
            return GameSettings.Instance.player2.getCardObjectInHand(cardID);
        else
            return null;
    }

    private void playSummons(CardObject currentCard, GameObject area, Deck player)
    {
        if (currentCard.data.cost <= player.spellPoints && player.table.Count < GameSettings.Instance.maxSummons)
        {
            player.spellPoints -= currentCard.data.cost;
            player.table.Add(currentCard);
            player.hand.Remove(currentCard);
            currentCard.CardDisplay.GetComponent<RectTransform>().SetParent(area.transform, true);
        }

    }

    private void attackSummons(CardObject currentCard, Deck player)
    {
        Deck target;
        if (player.playerID == 1)
            target = GameSettings.Instance.player2;
        else
            target = GameSettings.Instance.player1;

        Debug.Log("HandtoTable attack: " + player.playerID);

        GameSettings.Instance.engine.attack(currentCard, target.playerInfo);
    }

    private void playSpell(CardObject currentCard, Deck player)
    {
        Deck target;
        if (player.playerID == 1)
            target = GameSettings.Instance.player2;
        else
            target = GameSettings.Instance.player1;

        player.spellPoints -= currentCard.data.cost;
        GameSettings.Instance.engine.useCard(target.playerInfo, currentCard, player);
        player.hand.Remove(currentCard);
        Destroy(currentCard.CardDisplay);
        Destroy(currentCard);
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
