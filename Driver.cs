using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Driver : MonoBehaviour
{
    // Start is called before the first frame update
    public UserFunction userFunction;

    public DrawCoefficients drawcoefficients;
    //public ApplyFourierSeries applyFourierSeries;
    bool input_taken=false;
    bool fourier_series_calculated=false;

    int number_of_approximators;

    public GameObject fourier_series_drawer_prefab;
    public List<GameObject> fourier_series_drawers = new List<GameObject>();

    Dictionary<KeyCode, int> keyCodeDic = new Dictionary<KeyCode, int>();

    void Start()
    {
        /*
        The driver start starts the script for getting user input. The method returns the custom user function
        This custom user function is passed to apply fourier series to make the ball trace the fourier series of the function.

        */
        // Debug.Log("In driver");

        number_of_approximators = 8; //<=10
        for (int i = 48; i < 48+number_of_approximators; i++)
        {
            //Add keycode to dictionary
            KeyCode tempKeyCode = (KeyCode)i;
            keyCodeDic.Add(tempKeyCode, i-48);

            //Make a game_object from prefab
            GameObject new_fourier_series_drawer = Instantiate(fourier_series_drawer_prefab, new Vector3(0,0,0), Quaternion.identity);
            LineRenderer lr = new_fourier_series_drawer.GetComponent<LineRenderer>();
            Color c1 = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
                                UnityEngine.Random.Range(0.0f, 1.0f), 
                                UnityEngine.Random.Range(0.0f, 1.0f), 
                                1);

            lr.SetColors(c1,c1);
            fourier_series_drawers.Add(new_fourier_series_drawer);
        }


        // fourier_series_drawers = GameObject.FindGameObjectsWithTag("FourierSeries");
        
        

    }

    // IEnumerator ExecuteDriverSequence()
    // {
    //     userFunction.GetUserFunction();
    //     applyFourierSeries.CalculateFourierSeries(userFunction.function_to_transform);
    //     yield return null;
    // }

    // Update is called once per frame
    void Update()
    {

        if(!input_taken)
            input_taken=userFunction.check_for_input();

        else if(!fourier_series_calculated)
        {
            //applyFourierSeries.CalculateFourierSeries(userFunction.function_to_transform);
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
            //applyFourierSeries.AUpdate();
            for(int i=0;i<number_of_approximators;i++)
            {
                ApplyFourierSeries temp = fourier_series_drawers[i].GetComponent<ApplyFourierSeries>();
                temp.AUpdate(); 
            }

            drawcoefficients.AUpdate();

        }

        foreach (KeyValuePair<KeyCode, int> entry in keyCodeDic)
        {
            //Check if the keycode is pressed
            if (Input.GetKeyDown(entry.Key))
            {
                LineRenderer lr = fourier_series_drawers[entry.Value].GetComponent<LineRenderer>();
                Renderer r = fourier_series_drawers[entry.Value].GetComponent<Renderer>();
                lr.enabled=!lr.enabled;
                r.enabled=!r.enabled;
            }
        }

        // if(Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     LineRenderer lr = fourier_series_drawers[0].GetComponent<LineRenderer>();
        //     Renderer r = fourier_series_drawers[0].GetComponent<Renderer>();
        //     lr.enabled=!lr.enabled;
        //     r.enabled=!r.enabled;
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     LineRenderer lr = fourier_series_drawers[1].GetComponent<LineRenderer>();
        //     Renderer r = fourier_series_drawers[1].GetComponent<Renderer>();
        //     lr.enabled=!lr.enabled;
        //     r.enabled=!r.enabled;
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     LineRenderer lr = fourier_series_drawers[2].GetComponent<LineRenderer>();
        //     Renderer r = fourier_series_drawers[2].GetComponent<Renderer>();
        //     lr.enabled=!lr.enabled;
        //     r.enabled=!r.enabled;
        // }
    }
}
