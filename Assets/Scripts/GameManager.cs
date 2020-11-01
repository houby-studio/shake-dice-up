using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // This script handles connection and jobs between all components in this game
    // UI Controls
    // Detecting user input
    // Updating number of dice
    // Updating dice type
    // Updating dice colors
    // Keeping score and dice state
    
    [Header("Other Components")]
    public TextMeshProUGUI scoreNumber;
    public TextMeshProUGUI diceCountText;
    public GameObject mainMenu;
    public GameObject aboutMenu;
    public Slider diceCountSlider;
    public FlexibleColorPicker colorPicker;
    public Material diceMaterial;
    public Material dotMaterial;
    public Button dotColorSelected;
    public Button dotColorWhite;
    public Button dotColorBlack;
    public Image dotColorImage;
    public Image menuButtonImage;
    public Sprite menuButtonSprite;
    public Sprite restartButtonSprite;
    private Camera mainCamera;

    [Header("User Updated")]
    public int diceCount;
    public GameObject selectedDice;

    [Header("Physics")]
    public float maxSpeed;
    public float freezableMagnitudeLimit;

    // Shake Detection
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float shakeDetectionThreshold = 2.0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;

    // Keep Score and Dice objects
    private int score;
    private List<GameObject> dice = new List<GameObject>();
    private bool diceFrozen;

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

        mainCamera = Camera.main;
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

        // Check if user tapped on dice and freeze/unfreeze them
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            { 
                Ray ray = mainCamera.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        hit.collider.GetComponent<DiceControl>().ToggleFreeze();
                    }
                }
            }

        }

        // Throw dice when shake is detected
        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            ThrowDice();

        // Throw dice when Jump button is pressed
        if (Input.GetButtonDown("Jump"))
            ThrowDice();
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

    // Dice controls

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

    private void UnfreezeAll()
    {
        dice.ForEach(d => d.GetComponent<DiceControl>().Unfreeze());
        diceFrozen = false;
        menuButtonImage.sprite = menuButtonSprite;
    }

    // Color options

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
        foreach (GameObject die in dice)
        {
            die.GetComponent<DiceControl>().UpdateOutlineColor();
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

    // UI Controls

    public void ToggleMenu()
    {
        if (diceFrozen)
            UnfreezeAll();
        else
            mainMenu.SetActive(!mainMenu.activeSelf);
    }

    public void ToggleAbout()
    {
        aboutMenu.SetActive(!aboutMenu.activeSelf);
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

    public void UpdateButtonFunction()
    {
        // Check if any dice is frozen and toggle menu button and unfreeze all button
        if(dice.Any(d => d.GetComponent<DiceControl>().frozen))
        {
            diceFrozen = true;
            menuButtonImage.sprite = restartButtonSprite;
        } else
        {
            diceFrozen = false;
            menuButtonImage.sprite = menuButtonSprite;
        }
    }

    public void OpenDonateButton()
    {
        Application.OpenURL("https://paypal.me/HoubyStudio");
    }
}
