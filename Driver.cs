using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Driver : MonoBehaviour
{
    public UserFunction userFunction;
    public DrawCoefficients drawcoefficients;
    bool input_taken=false;
    bool fourier_series_calculated=false;
    int number_of_approximators;
    public GameObject fourier_series_drawer_prefab;
    public List<GameObject> fourier_series_drawers = new List<GameObject>();
    Dictionary<KeyCode, int> keyCodeDic = new Dictionary<KeyCode, int>();

    void Start()
    {
        /*
        Instantiates required number of fourier approximators and 
        dictionary of numeric keys for toggling display of function on/off.
        */

        number_of_approximators = 8; //<=10
        for (int i = 48; i < 48+number_of_approximators; i++)
        {
            //Add keycode to dictionary
            KeyCode tempKeyCode = (KeyCode)i;
            keyCodeDic.Add(tempKeyCode, i-48);

            //Make a game_object from prefab
            GameObject new_fourier_series_drawer = Instantiate(fourier_series_drawer_prefab, new Vector3(0,0,0), Quaternion.identity);
            LineRenderer lr = new_fourier_series_drawer.GetComponent<LineRenderer>();

            //Give a random colour to the line
            Color c1 = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
                                UnityEngine.Random.Range(0.0f, 1.0f), 
                                UnityEngine.Random.Range(0.0f, 1.0f), 
                                1);

            lr.SetColors(c1,c1);
            fourier_series_drawers.Add(new_fourier_series_drawer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Performs the various tasks in sequence
        1) Takes user input
        2) Calculates fourier series for all approximators
        3) Calls FunctionUpdate for each approximator
        4) Checks for display toggled on/off for a particular approximator.
        */

        if(!input_taken)
            input_taken=userFunction.CheckForInput();

        else if(!fourier_series_calculated)
        {
            for(int i=0;i<number_of_approximators;i++)
            {
                ApplyFourierSeries temp = fourier_series_drawers[i].GetComponent<ApplyFourierSeries>();
                temp.CalculateFourierSeries(userFunction.function_to_transform , (int)Math.Pow(2,i+2)); 
            }
            drawcoefficients.CalculateFourierSeries(userFunction.function_to_transform , 16);
            fourier_series_calculated=true;
        }
        else
        {
            for(int i=0;i<number_of_approximators;i++)
            {
                ApplyFourierSeries temp = fourier_series_drawers[i].GetComponent<ApplyFourierSeries>();
                temp.FunctionUpdate(); 
            }

            drawcoefficients.FunctionUpdate();
        }

        foreach (KeyValuePair<KeyCode, int> entry in keyCodeDic)
        {
            //Check if the keycode is pressed
            if (Input.GetKeyDown(entry.Key))
            {
                //Toggle visibility of GameObject and line renderer

                LineRenderer lr = fourier_series_drawers[entry.Value].GetComponent<LineRenderer>();
                Renderer r = fourier_series_drawers[entry.Value].GetComponent<Renderer>();
                lr.enabled = ! lr.enabled;
                r.enabled = ! r.enabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Toggle visibility of DrawCoefficients
            drawcoefficients.ToggleVisibility();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Toggle visibility of DrawCoefficients
            userFunction.ToggleVisibility();
        }

    }
}
