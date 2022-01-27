using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCores : MonoBehaviour
{
    private PlayerCollects playerCollects;



    void awake()
    {
        playerCollects = GetComponent<PlayerCollects>();
        
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
        if (other.name == "Player") // Player collides with core and core will disappear
        {
            playerCollects.whiteBlackCores += 1;
            Destroy(this.gameObject);
        }

    }


}
