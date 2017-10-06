using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WPException
{
    public int travelArea;
    public int[] newArea; 
}

public class Waypoints : MonoBehaviour
{
    /*
    This script checks which area should be moved to, depending on what the newArea (final destination) is. 
    If the final destination is not adjacent, it will move to the area(s) in between
    */

    public Transform[] wayPoints;
    public int thisWayPoint;

    public WPException[] WPE;
	
    // called from the Human script, to see what area should be moved to
    // these values are set in the inspector
    public int SetNextWayPoint(int newArea)
    {
        int nextWP = newArea;

        if (newArea != thisWayPoint)
        {
            for (int i = 0; i < WPE.Length; i++)
            {
                for (int c = 0; c < WPE[i].newArea.Length; c++)
                {
                    if (newArea == WPE[i].newArea[c])
                    {
                        nextWP = WPE[i].travelArea;
                    }
                }
            }
        }
        return nextWP;
    }
}
