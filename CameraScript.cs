﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public float inputZ=0.0f;
    public float inputX=0.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(!Input.GetKey(KeyCode.Space))
        {
        //Debug.Log("In the cam angle changing code");
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }


        inputZ= Input.GetAxis("Vertical");
        inputX=Input.GetAxis("Horizontal");
        if(inputZ!=0)
        {
            transform.position+=transform.forward*50*inputZ*Time.deltaTime;
        }
        if(inputX!=0)
        {
            transform.position+=transform.right*50*inputX*Time.deltaTime;
        }



    }
}

