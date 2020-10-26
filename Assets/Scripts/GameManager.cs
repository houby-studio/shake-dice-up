using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Other Components")]
    public TextMeshProUGUI scoreNumber;
    public GameObject mainMenu;

    [Header("User Updated")]
    public int diceCount;
    public GameObject selectedDice;

    [Header("Physics")]
    public float maxSpeed;

    // Shake Detection
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float shakeDetectionThreshold = 2.0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;

    // Keep Score and Dice objects
    private int score;
    private List<GameObject> dice = new List<GameObject>();

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
        RecreateDice();
    }

    private void Update()
    {
        // Calculate current acceleration value of device
        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        // Throw dice when shake is detected
        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            ThrowDice();

        // Throw dice when Jump button is pressed
        if (Input.GetButtonDown("Jump"))
            ThrowDice();
    }

    public void RecreateDice()
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

    private GameObject CreateNewDice()
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

    public void ThrowDice()
    {
        foreach (GameObject die in dice)
        {
            die.GetComponent<DiceControl>().ThrowDice();
        }
    }

    public void ToggleMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
}
