using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConeView : MonoBehaviour
{
    //THe angle that the "Player" will be capable to see in front of him.
    [SerializeField]
    private float cutoff = 45f;

    //Return true if he can see an apple in his front
    public bool Sight(Vector3 inputPoint)
    {
        //Get the cosene of the angle between the foward vector from player and the vector from the apple
        float cosAngle = Vector3.Dot((inputPoint - this.transform.position).normalized, this.transform.forward);
        //transform the angle from radians to degrees
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;

        //If the angle is minor than the cutoff (45 degres) then the "Player" is seeing the apple.
        if (angle < cutoff)
        {
            //Draw a line from the player to the apple.
            Debug.DrawLine(transform.position, inputPoint, Color.red);
            //return true.
            return true;
        }
        //return false.
        return false;
    }
}
