using System.Collections;
using UnityEngine;

public class ColorCores : MonoBehaviour
{
    private PlayerCollects playerCollects;
    // public GameObject player;
   
    void awake()
    {
        
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") // Player collides with core and core will disappear
        {
            playerCollects = other.gameObject.GetComponent<PlayerCollects>();
            playerCollects.whiteBlackCores += 1;
            Destroy(this.gameObject);
        }

    }

    private IEnumerator WaitForSoundToPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.gameObject.SetActive(false);
    }
}
