using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollects : MonoBehaviour
{

    public int blackCores;
    public int whiteCores;

    private GUIManager guiManager;

    void awake()
    {
        guiManager = FindObjectOfType<GUIManager>();

        whiteCores = 0;
        blackCores = 0;

    }
    // Start is called before the first frame update
    void Start()
    {
       
      
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void AddCore(bool isLightCore)
    {
        if (isLightCore) whiteCores++;
        else blackCores++;

        guiManager.UpdateCoresOnHud(blackCores, whiteCores);
    }


}
