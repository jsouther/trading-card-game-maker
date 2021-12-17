using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpellpoints : MonoBehaviour
{
    public Text SpellpointsText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpellpointsText.text = string.Format("{0}",
            GameSettings.Instance.player2.spellPoints.ToString());
    }
}
