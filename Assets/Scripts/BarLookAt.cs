using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLookAt : MonoBehaviour
{
 
    //This will make the bar look at te camera.
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
