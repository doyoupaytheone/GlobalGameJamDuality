// Code created by Akeem Roberts 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WorldSwitch : MonoBehaviour
{
    
    public GameObject black;
    
    public GameObject White;

    public bool worldBlack = false;

    private void Awake()
    {
        black.SetActive(false); //Sets the Obsidian world false first when level loads
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        WorldSwapBlack();
        WorldSwapWhite();
    }



    void WorldSwapBlack()
    {
      
            if (Input.GetKeyDown(KeyCode.E))
            {
                black.SetActive(true);
                White.SetActive(false);
                
            }
       
    }

    void WorldSwapWhite()
    {
       
            if (Input.GetKeyDown(KeyCode.Q))
            {
                White.SetActive(true);
                black.SetActive(false);
            }
      
    }
}
