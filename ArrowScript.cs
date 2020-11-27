using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArrowScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 look_at = new Vector3(0,10,-10);
        Vector3 center_at = new Vector3(0,10,10);

        float scale_factor = Vector3.Distance(look_at, center_at);
        
        Vector3 s = new Vector3(scale_factor,scale_factor,scale_factor);
        transform.position = center_at;
        transform.LookAt(look_at);
        transform.localScale = s; 
        
    }

}
