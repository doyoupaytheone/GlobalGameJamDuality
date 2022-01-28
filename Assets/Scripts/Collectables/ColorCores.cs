using System.Collections;
using UnityEngine;

public class ColorCores : MonoBehaviour
{
    private PlayerCollects playerCollects;
    private AudioSource audioSource;

    public bool isLightCore;

    void awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            playerCollects.AddCore(isLightCore);
            audioSource.Play();
            StartCoroutine(WaitForSoundToPlay(audioSource.clip.length)); //Waits until the audio clip stops playing before disabling the core
        }

    }

    private IEnumerator WaitForSoundToPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.gameObject.SetActive(false);
    }
}
