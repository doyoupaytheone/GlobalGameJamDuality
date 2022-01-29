using System.Collections;
using UnityEngine;

public class ColorCores : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource audioSource;

    private PlayerCollects playerCollects;
    // public GameObject player;

    public bool isLightCore;
   
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
        if (other.tag == "Player" || other.gameObject.CompareTag("Projectile")) // Player collides with core and core will disappear
        {
            playerCollects = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollects>();
            playerCollects.AddCore(isLightCore);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            if (audioSource != null)
            {
                audioSource.Play();
                audioSource.clip = clip;
                WaitForSoundToPlay(audioSource.clip.length);
            }
            else WaitForSoundToPlay(0);
        }

    }

    private IEnumerator WaitForSoundToPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.gameObject.SetActive(false);
    }
}
