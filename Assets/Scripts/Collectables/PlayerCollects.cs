using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollects : MonoBehaviour
{

    public int whiteBlackCores;

    public TextMeshProUGUI coreAmount;
    void awake()
    {
        whiteBlackCores = 0;

    }
    // Start is called before the first frame update
    void Start()
    {
       
      
    }

    // Update is called once per frame
    void Update()
    {
        coreAmount.text = whiteBlackCores.ToString();
        
    }

 




}
