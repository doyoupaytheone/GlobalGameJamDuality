using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollects : MonoBehaviour
{

    public int whiteCores = 0;
    public int blackCores = 0;

    // public TextMeshProUGUI coreAmount;
    private GUIManager guiManager;

    private void Awake()
    {
        guiManager = FindObjectOfType<GUIManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
       
      
    }

    // Update is called once per frame
    void Update()
    {
         // coreAmount.text = whiteBlackCores.ToString();
        
    }

    public void AddCore(bool isLightCore)
    {
        if (isLightCore) whiteCores++;
        else blackCores++;

        guiManager.UpdateCoresOnHud(blackCores, whiteCores);
    }


}
