using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZone : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        // Obtain component from root object of collision trigger source, that is an Dice object with DiceControl script attached
        DiceControl diceObject = col.gameObject.transform.root.GetComponent<DiceControl>();
        switch (col.gameObject.name)
        {
            case "Trigger1":
                diceObject.UpdateScore(6);
                break;
            case "Trigger2":
                diceObject.UpdateScore(5);
                break;
            case "Trigger3":
                diceObject.UpdateScore(4);
                break;
            case "Trigger4":
                diceObject.UpdateScore(3);
                break;
            case "Trigger5":
                diceObject.UpdateScore(2);
                break;
            case "Trigger6":
                diceObject.UpdateScore(1);
                break;
            default:
                break;
        }
        GameManager.instance.UpdateScore();
    }
}
