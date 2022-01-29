using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreCheck : MonoBehaviour
{
    private CoreWall coreWall;
    private PlayerCollects playerCollects;
    
    // Start is called before the first frame update
    void Start()
    {
        coreWall = GetComponent<CoreWall>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.name == "Player") // Player collides with core and core will disappear
        {
            Debug.Log("CoreCheck");
            playerCollects = other.gameObject.GetComponent<PlayerCollects>();
            if (playerCollects.whiteCores + playerCollects.blackCores == coreWall.coresRequired)
            {
                Debug.Log("Bool True");
               // coreWall.allCoresAcquired = true;
            }
        }
    }
}
