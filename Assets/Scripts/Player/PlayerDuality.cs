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

    public int dualityTimer = 0;
    public bool isInLightMode = true;

    private SpriteRenderer background;

    private void Awake() => background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();

    private void Start()
    {
        //Initializes the max time for each timer in milliseconds
        darkSlider.maxValue = maxDualityTime;
        lightSlider.maxValue = maxDualityTime;

        darkObjects.SetActive(false); //Sets the Obsidian world false first when level loads

        InvokeRepeating("AddTimeToTimer", 0.1f, 0.1f); //Repeats method every 0.1 seconds
    }

    private void Update()
    {
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
            //Toggles the appropriate world objects
            darkObjects.SetActive(isInLightMode);
            lightObjects.SetActive(!isInLightMode);

            //Toggles the background color
            background.enabled = isInLightMode;

            ToggleType();
        }
    }
}
