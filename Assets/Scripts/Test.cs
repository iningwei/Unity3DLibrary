using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    void Start()
    { 
        Color rgb2 = new Color(69f / 255, 139f / 255f, 116f / 255f);
        string xx=rgb2.RGBToHex();
        Debug.Log(xx);
    }

  
}
