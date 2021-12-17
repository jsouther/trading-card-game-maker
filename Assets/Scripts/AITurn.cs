using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AITurn : MonoBehaviour
{
    public static AITurn AITurnEngine;
    // Start is called before the first frame update
    void Start()
    {
        AITurnEngine = this;
    }

    public IEnumerator doAITurn()
    {
        yield return new WaitForSeconds(3.0f);
        while (playCards())
        {
            yield return new WaitForSeconds(1.0f);
        }
        while (makeAttacks())
        {
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(2.0f);
        GameSettings.Instance.engine.EndTurn();
    }

    private IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // Play Cards From Hand at random -> 
    private bool playCards()
    {
        Deck player = GameSettings.Instance.player2;
        GameObject area = GameObject.Find("EnemyArea/EnemySummons");
        foreach (CardObject card in player.hand)
        {
            if (card.data.summon)
            {
                if (card.data.cost <= player.spellPoints 
                    && player.table.Count < GameSettings.Instance.maxSummons)
                {
                    // Play the card
                    player.spellPoints -= card.data.cost;
                    player.table.Add(card);
                    player.hand.Remove(card);
                    card.CardDisplay.GetComponent<RectTransform>().SetParent(area.transform, true);
                    Debug.Log("Playing Summon");
                    return true;
                }
            }
            
        }
        Debug.Log("No Summons left to play");
        return false;
    }

    
    private bool makeAttacks()
    {
        Deck player1 = GameSettings.Instance.player1;
        Deck player2 = GameSettings.Instance.player2;
        foreach (CardObject card in player2.table)
        {
            int numSummons = player1.table.Count;
            if (numSummons > 0 && card.data.canAttack == true)
            {
                //Make an attack
                int target = Random.Range(0, numSummons);
                Debug.Log(player1.table[target].data.life);
                Debug.Log("Making an attack");
                GameSettings.Instance.engine.attack(card, player1.table[target]);
                Debug.Log(player1.table[target].data.life);         
                cleanup(player1.table[target], player1);
                return true;
            }
            if (card.data.canAttack == true)
            {
                Debug.Log("attacking player");
                GameSettings.Instance.engine.attack(card, player1.playerInfo);
            }
        }
        Debug.Log("No Attacks");
        return false;
    }
    // Updates the targets cardDisplay after an attack
    private void cleanup(CardObject target, Deck owner)
    {
        if (target.data.life <= 0)
        {
            owner.table.Remove(target);
            Destroy(target.CardDisplay);
            Destroy(target);
        } else
        {
            Text foundLife = target.CardDisplay.transform.Find("SummonPanel/lblLife").GetComponent<Text>();
            foundLife.text = target.data.life.ToString();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
