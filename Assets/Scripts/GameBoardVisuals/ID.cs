using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ID : MonoBehaviour
{
    public int myID;
    public static List<ID> allIDs = new List<ID>();
    void Start()
    {
        allIDs.Add(this);
    }

    public static GameObject getByID(int ID)
    {
        for(int i = 0; i < allIDs.Count; i++)
        {
            if (allIDs[i].myID == ID)
            {
                return allIDs[i].gameObject;
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
