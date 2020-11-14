using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZone : MonoBehaviour
{

    // This script updates score based on the Trigger which collides with the ground Trigger

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "DiceTrigger")
        { 
            // Obtain component from root object of collision Trigger source, that is an Dice object with DiceControl script attached and update score
            col.gameObject.transform.root.GetComponent<DiceControl>().UpdateScore(GetDiceScore(col));
            GameManager.instance.UpdateScore();
        }
    }

    int GetDiceScore(Collider col)
    {
        switch (col.gameObject.name)
        {
            case "T1":
                return 1;
            case "T2":
                return 2;
            case "T3":
                return 3;
            case "T4":
                return 4;
            case "T5":
                return 5;
            case "T6":
                return 6;
            case "T7":
                return 7;
            case "T8":
                return 8;
            case "T9":
                return 9;
            case "T10":
                return 10;
            default:
                return 0;
        }
    }
}
