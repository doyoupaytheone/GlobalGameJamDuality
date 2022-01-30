using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoreWall : MonoBehaviour
{

    public int coresRequired;
    private PlayerCollects playerCollects;
    public SpriteRenderer lockSprite;
    public GameObject wall;
    public GameObject player;

    public float fadeDelay = 3f;
    public float alphaValue = 0;
    public bool destroyGOWall = false;

    // public bool allCoresAcquired = false;
    public Canvas CoresRequired;
    public TextMeshProUGUI CoresRequiredText;
    

    public SpriteRenderer spriteRender; // Use sprite renderer to fade wall

    void awake()
    {
        spriteRender = GetComponent<SpriteRenderer>(); //Need to reference sprite rend

        CoresRequired.enabled = false;
       
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CollectedCoresRequired();    
    }

    void CollectedCoresRequired()
    {
        playerCollects = player.gameObject.GetComponent<PlayerCollects>();
        
        if (playerCollects.whiteCores + playerCollects.blackCores >= coresRequired )
        {
            StartCoroutine(WallFade(alphaValue, fadeDelay));
            Debug.Log("Cores need acquired");
        }
        
    }

    IEnumerator WallFade(float aValue, float fadeTime)
    {
        float alpha = lockSprite.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(lockSprite.color.r, lockSprite.color.g, lockSprite.color.b, Mathf.Lerp(alpha, aValue, t));
            lockSprite.color = newColor;
            yield return null;
            Debug.Log("Lock fading");
        }

        alpha = spriteRender.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(spriteRender.color.r, spriteRender.color.g, spriteRender.color.b, Mathf.Lerp(alpha, aValue, t));
            spriteRender.color = newColor;
            yield return null;
            Debug.Log("Wall fading");
        }

        if (destroyGOWall) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerCollects = other.gameObject.GetComponent<PlayerCollects>();

            if (playerCollects.whiteCores + playerCollects.blackCores < coresRequired)
            {
                CoresRequired.enabled = true;
                CoresRequiredText.text = "Cores Needed to Proceed: " + coresRequired.ToString();
            }
            else
            {
                CollectedCoresRequired();
            }
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CoresRequired.enabled = false;
    }


}
