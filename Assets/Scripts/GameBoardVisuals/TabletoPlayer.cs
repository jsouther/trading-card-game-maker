using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabletoPlayer : MonoBehaviour
{
    public void OnDrop2(PointerEventData eventData)
    {

        GameObject droppedCard = eventData.pointerDrag;
        GameObject area = eventData.pointerEnter;
        Deck player;
        if (droppedCard.transform.parent.name == "PlayerHand")
        {
            player = GameSettings.Instance.player1;
            area = GameObject.Find("PlayerArea/Summons");
        }
        else
        {
            player = GameSettings.Instance.player2;
            area = GameObject.Find("PlayerArea/EnemySummons");
        }

        // Remove Card from Hand
        int cardID = droppedCard.GetComponent<ID>().myID;
        CardObject currentCard = player.getCardObjectInHand(cardID);

        // Check if the player has enough spellpoints and has space to play the summon
        if (!currentCard.played)
        {
            currentCard.played = true;
            if (currentCard.data.summon)
            {
                Debug.Log("called");
                if (currentCard.transform.parent.name == "PlayerHand" || currentCard.transform.parent.name == "EnemyHand")
                    playSummons(currentCard, area, player);
                else
                    attackSummons(currentCard, player);
            }
            else
            {
                playSpell(currentCard, player);
            }
        }
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
