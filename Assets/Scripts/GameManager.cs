using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Other Components")]
    public TextMeshProUGUI scoreNumber;

    [Header("User Updated")]
    public int diceCount;
    public GameObject selectedDice;

    // Shake Detection
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    float lowPassFilterFactor;
    Vector3 lowPassValue;

    // Keep Score and Dice objects
    private int score;
    public List<GameObject> dice = new List<GameObject>();

    // Instantiate
    public static GameManager instance;

    private void Awake()
    {
        // Initialize GameManager Instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Initialize variables for shake detection
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    private void Start()
    {
        foreach (GameObject die in dice)
        {
            Destroy(die);
        }
        for (int x = 0; x < diceCount; x++)
        {
            CreateNewDice();
        }
    }

    private void Update()
    {
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            Debug.Log("Shake event detected at time " + Time.time);
            foreach(GameObject die in dice)
            {
                die.GetComponent<DiceControl>().ThrowDice();
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            foreach (GameObject die in dice)
            {
                die.GetComponent<DiceControl>().ThrowDice();
            }
        }
    }

    GameObject CreateNewDice()
    {
        GameObject obj = Instantiate(selectedDice);
        dice.Add(obj);
        return obj;
    }

    public void UpdateScore()
    {
        int tempScore = 0;
        foreach (GameObject die in dice)
        {
            tempScore += die.GetComponent<DiceControl>().number;
        }
        score = tempScore;
        scoreNumber.text = score.ToString();
    }
}
