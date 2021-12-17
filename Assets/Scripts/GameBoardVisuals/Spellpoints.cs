using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Spellpoints : MonoBehaviour
{
    public Text SpellpointsText;
    public int testTotalSpellpoints;
    public Image[] SpellpointsImageArray;

    // Start is called before the first frame update
    void Start()
    {
//        engine = gameObject.AddComponent(typeof(GameEngine)) as GameEngine;

    }

    // Update is called once per frame
    void Update()
    {
        SpellpointsText.text = string.Format("{0}",
            GameSettings.Instance.player1.spellPoints.ToString());
    }
}
