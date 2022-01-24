using UnityEngine;
using UnityEngine.UI;

public class PlayerDuality : MonoBehaviour
{
    [Tooltip("The time in milliseconds it takes to max out either side of the duality meter.")]
    [SerializeField] private int maxDualityTime;
    [SerializeField] private Slider darkSlider;
    [SerializeField] private Slider lightSlider;
    [SerializeField] private GameObject darkObjects;
    [SerializeField] private GameObject lightObjects;
    [SerializeField] private GameObject[] darkEnemies;
    [SerializeField] private GameObject[] lightEnemies;

    public int dualityTimer = 0;
    public bool isInLightMode = false;

    private SpriteRenderer background;
    private Animator animator;

    private void Awake()
    {
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Initializes the max time for each timer in milliseconds
        darkSlider.maxValue = maxDualityTime;
        lightSlider.maxValue = maxDualityTime;

        //Sets the Obsidian world false first when level loads
        darkObjects.SetActive(false);
        ToggleEnemyVisibility(darkEnemies, false);
        ToggleEnemyVisibility(lightEnemies, true);

        InvokeRepeating("AddTimeToTimer", 0.1f, 0.1f); //Repeats method every 0.1 seconds
    }

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen)
            WorldSwap(); //Receives player input to change type
    }

    private void ToggleType()
    {
        dualityTimer = 0;
        isInLightMode = !isInLightMode;
        lightSlider.value = 0;
        darkSlider.value = 0;
    }

    private void AddTimeToTimer()
    {
        //Makes sure the object is still active and the timer has not reached the max
        if (this.gameObject.activeSelf && dualityTimer < maxDualityTime)
        {
            dualityTimer++; //Updates the timer
            UpdateHudDisplay(); //Updates the display
        }
    }

    private void UpdateHudDisplay()
    {
        if (isInLightMode) lightSlider.value = dualityTimer;
        else darkSlider.value = dualityTimer;
    }

    private void WorldSwap()
    {
        if (Input.GetKeyDown(PlayerPrefData.Interact))
        {
            if (animator != null) animator.SetTrigger("ability"); //Plays the ability animation

            //Toggles the appropriate world objects
            darkObjects.SetActive(isInLightMode);
            lightObjects.SetActive(!isInLightMode);

            //Toggles the appropriate enemies
            ToggleEnemyVisibility(darkEnemies, false);
            ToggleEnemyVisibility(lightEnemies, true);

            //Toggles the background color
            background.enabled = isInLightMode;

            ToggleType();
        }
    }

    private void ToggleEnemyVisibility(GameObject[] enemies, bool typeIsLight)
    {
        foreach (GameObject e in enemies)
        {
            if (typeIsLight)
            {
                e.GetComponent<SpriteRenderer>().enabled = !isInLightMode;
                e.GetComponentInChildren<Canvas>().enabled = !isInLightMode;
            }
            else
            {
                e.GetComponent<SpriteRenderer>().enabled = isInLightMode;
                e.GetComponentInChildren<Canvas>().enabled = isInLightMode;
            }
        }
    }
}
