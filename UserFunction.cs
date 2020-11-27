using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserFunction : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Vector3> positions=new List<Vector3>();
    private List<double> positon_distance=new List<double>();
    public Func<double,double[]> function_to_transform;
    public LineRenderer custom_function_line;

    void Start()
    {
        custom_function_line=GetComponent<LineRenderer>();
        custom_function_line.positionCount=0;
        
        // StartCoroutine(ExecuteSequence());
        // //Func<double,double[]> function_to_transform=ConvertListtoFunction();

        // Debug.Log("In UserFunction");

        
    }

    IEnumerator ExecuteSequence()
    {
        yield return StartCoroutine(GetPositionList());
        ConvertListtoFunction();
    }

    IEnumerator GetPositionList()
    {

        while(!Input.GetKeyDown(KeyCode.Return))
        {
            
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane+10;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                positions.Add(worldPosition);
                UpdateLineRenderer(worldPosition);
            }
            yield return null;
        }

        positions.Add(positions[0]);
        UpdateLineRenderer(positions[0]);
    }

    public bool check_for_input()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            positions.Add(positions[0]);
            UpdateLineRenderer(positions[0]);
            ConvertListtoFunction();
            return(true);
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane+10;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                positions.Add(worldPosition);
                UpdateLineRenderer(worldPosition);
            }
            return(false);
        }
        
    }

    private void UpdateLineRenderer(Vector3 position)
    {
        custom_function_line.positionCount++;
        custom_function_line.SetPosition(custom_function_line.positionCount-1,position);
    }


    private void ConvertListtoFunction()
    {
        Debug.Log("In ConvertListtoFunction");
        
        double total_distance=SetPositionDistances();

        Debug.Log("Done distances");
        
        double[] func(double time)
        {
            /*
            This function finds proper interval of the function and then returns the interpolated
            double[] position back
            */
            double[] ret_val=new double[3];
            if(time<0 || time>1)
                return(ret_val);

            //Performing Binary Search
            int start=0;
            int end=positon_distance.Count-1;

            int mid;
            while(start+1<end)
            {
                mid=(start+end)/2;
                if(positon_distance[mid]>time)
                    end=mid;
                else
                    start=mid;
            }

            //Interpolating to find return val
            Vector3 addvec=(positions[end]-positions[start])*(float)((time-positon_distance[start])/(positon_distance[end]-positon_distance[start]));
            Vector3 return_vector=positions[start]+addvec;

            ret_val[0]=return_vector.x;
            ret_val[1]=return_vector.y;
            ret_val[2]=return_vector.z;

            return(ret_val);
        }

        function_to_transform=func;
    }

    private double SetPositionDistances()
    {
        /*
        SetPositionDistances() sets the position_distance list with distance of the points along the curve
        and returns the total distance covered
        */
        Debug.Log("In SetPositionDistances");
        Debug.Log("Positions count is "+positions.Count);
        double distance_till_now=0;
        positon_distance.Add(0);
        for(int i=1;i<positions.Count;i++)
        {

            distance_till_now+=Vector3.Distance(positions[i-1],positions[i]);
            positon_distance.Add(distance_till_now);
        }

        //Normalised distance for function
        for(int i=0;i<positions.Count;i++)
        {
            positon_distance[i]/=distance_till_now;
        }

        Debug.Log("Done SetPositionDistances");
        return(distance_till_now);

    }

    
}
