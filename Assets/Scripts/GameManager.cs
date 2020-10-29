using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Other Components")]
    public TextMeshProUGUI scoreNumber;
    public TextMeshProUGUI diceCountText;
    public GameObject mainMenu;
    public Slider diceCountSlider;
    public FlexibleColorPicker colorPicker;
    public Material diceMaterial;
    public Material dotMaterial;
    public Button dotColorSelected;
    public Button dotColorWhite;
    public Button dotColorBlack;
    public Image dotColorImage;

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
        // Initialize player preferences
        // Dice dot color
        if (PlayerPrefs.GetInt("IsDotWhite") == 1)
            dotMaterial.color = new Color(1f, 1f, 1f);
        else
            dotMaterial.color = new Color(0f, 0f, 0f);
        // Dice sides color
        if (PlayerPrefs.HasKey("diceColorR") && PlayerPrefs.HasKey("diceColorG") && PlayerPrefs.HasKey("diceColorB"))
        {
            diceMaterial.color = new Color(PlayerPrefs.GetFloat("diceColorR"), PlayerPrefs.GetFloat("diceColorG"), PlayerPrefs.GetFloat("diceColorB"));
        }
        // Dice count
        if (PlayerPrefs.HasKey("diceCount"))
        {
            diceCountSlider.value = PlayerPrefs.GetInt("diceCount");
            UpdateDiceAmount();
        }
            
        // Create dice
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

    private void RecreateDice()
    {
        foreach (GameObject die in dice)
        {
            Destroy(die);
        }
        dice.Clear();
        for (int x = 0; x < diceCount; x++)
        {
            GameObject obj = Instantiate(selectedDice);
            dice.Add(obj);
        }
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

    public void UpdateDiceAmount()
    {
        int newDiceCount = (int)Mathf.RoundToInt(diceCountSlider.value);
        diceCount = newDiceCount;
        diceCountText.text = newDiceCount.ToString();
        PlayerPrefs.SetInt("diceCount", (int)Mathf.RoundToInt(diceCountSlider.value));
        RecreateDice();
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

    public void ToggleColorMenu()
    {
        colorPicker.gameObject.SetActive(!colorPicker.gameObject.activeSelf);
        if (colorPicker.gameObject.activeSelf)
        {
            colorPicker.startingColor = diceMaterial.color;
            colorPicker.color = diceMaterial.color;
            if (dotMaterial.color == new Color(1f, 1f, 1f))
            {
                dotColorSelected = dotColorWhite;
                dotColorImage.color = new Color(1f, 1f, 1f);
                PlayerPrefs.SetInt("IsDotWhite", 1);
            }
            else
            {
                dotColorSelected = dotColorBlack;
                dotColorImage.color = new Color(0f, 0f, 0f);
                PlayerPrefs.SetInt("IsDotWhite", 0);
            }
            dotColorSelected.interactable = false;
        }
    }

    public void SetDiceDotColor(Button btn)
    {
        dotColorSelected.interactable = true;
        dotColorSelected = btn;
        btn.interactable = false;
        if (btn.name == "DiceDotWhiteButton")
        {
            dotMaterial.color = new Color(1f, 1f, 1f);
            dotColorImage.color = new Color(1f, 1f, 1f);
            PlayerPrefs.SetInt("IsDotWhite", 1);

        } else
        {
            dotMaterial.color = new Color(0f, 0f, 0f);
            dotColorImage.color = new Color(0f, 0f, 0f);
            PlayerPrefs.SetInt("IsDotWhite", 0);
        }
    }

    public void SetColorPickerValue()
    {
        diceMaterial.color = colorPicker.color;
        PlayerPrefs.SetFloat("diceColorR", colorPicker.color.r);
        PlayerPrefs.SetFloat("diceColorG", colorPicker.color.g); 
        PlayerPrefs.SetFloat("diceColorB", colorPicker.color.b);
        ToggleColorMenu();
    }
}
