using UnityEngine;

public class PlayerDualityMenu : MonoBehaviour
{
    [SerializeField] private GameObject darkObjects;
    [SerializeField] private GameObject lightObjects;
    [SerializeField] private GameObject[] darkEnemies;
    [SerializeField] private GameObject[] lightEnemies;

    public bool isInLightMode = false;

    private SpriteRenderer background;
    private Animator animator;

    private void Awake()
    {
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    //Sets the Obsidian world false first when level loads
    private void Start() => darkObjects.SetActive(false);

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen && Input.GetKeyDown(PlayerPrefData.Interact)) WorldSwap(); //Receives player input to change type
    }

    private void ToggleType() => isInLightMode = !isInLightMode;

    private void WorldSwap()
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

    private void ToggleEnemyVisibility(GameObject[] enemies, bool typeIsLight)
    {
        foreach (GameObject e in enemies)
        {
            if (typeIsLight) e.GetComponent<SpriteRenderer>().enabled = !isInLightMode;
            else e.GetComponent<SpriteRenderer>().enabled = isInLightMode;
        }
    }
}
