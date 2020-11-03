using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZone : MonoBehaviour
{

    // This script updates score based on the trigger which collides with the ground trigger

    private int newScore = 0;

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "DiceTrigger")
        { 
            // Obtain component from root object of collision trigger source, that is an Dice object with DiceControl script attached
            DiceControl diceObject = col.gameObject.transform.root.GetComponent<DiceControl>();
            switch (diceObject.sides)
            {
                case 6:
                    newScore = SixSidedDice(col);
                    break;
                case 8:
                    newScore = EightSidedDice(col);
                    break;
                default:
                    break;
            }
            //Update score on dice and update total score
            diceObject.UpdateScore(newScore);
            GameManager.instance.UpdateScore();
        }
    }

    int SixSidedDice(Collider col)
    {
        // The trigger happens on the bottom, but the thrown number is on the other side of the dice
        switch (col.gameObject.name)
        {
            case "Trigger1":
                return 6;
            case "Trigger2":
                return 5;
            case "Trigger3":
                return 4;
            case "Trigger4":
                return 3;
            case "Trigger5":
                return 2;
            case "Trigger6":
                return 1;
            default:
                return 0;
        }
    }

    int EightSidedDice(Collider col)
    {
        // The trigger happens on the bottom, but the thrown number is on the other side of the dice
        switch (col.gameObject.name)
        {
            case "Trigger1":
                return 8;
            case "Trigger2":
                return 7;
            case "Trigger3":
                return 6;
            case "Trigger4":
                return 5;
            case "Trigger5":
                return 4;
            case "Trigger6":
                return 3;
            case "Trigger7":
                return 2;
            case "Trigger8":
                return 1;
            default:
                return 0;
        }
    }
}
