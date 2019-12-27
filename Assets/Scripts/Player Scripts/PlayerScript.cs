using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script will control the character.
public class PlayerScript : MonoBehaviour
{
    //Starvation level controls if the character lever of starvation, 0 no starvation, 1 starvation, 2 extreme starvation.
    [SerializeField]
    private int starvationLevel = 0;
    //Control death, false = alive, true = you are death... srry.
    private bool death = false;
    //THe angle that the "Player" will be capable to see in front of him.
    [SerializeField]
    private float cutoff = 45f;
    //List of apples that the AI will know of.
    protected List<GameObject> appleList = new List<GameObject>();

    private void Update()
    {
        StartCoroutine(StartFinding());
    }

    //Update bar situation to player
    public void StatusUpdate(string barName, int status)
    {
        if (barName == "Starve")
        {
            starvationLevel = status;
        }
    }

    //Return true if he can see an apple in his front
    public bool Sight(Vector3 inputPoint, GameObject apple)
    {
        //Get the cosene of the angle between the foward vector from player and the vector from the apple
        float cosAngle = Vector3.Dot((inputPoint - this.transform.position).normalized, this.transform.forward);
        //transform the angle from radians to degrees
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
        //Distance between player and apple.
        float distance = Vector3.Distance(this.transform.position, apple.transform.position);

        //If the angle is minor than the cutoff (45 degres) and the distance is minor than 15.0f then the "Player" is seeing the apple.
        if (angle < cutoff && distance < 15.0f)
        {
            //Insert apple on the list.
            InsertAppleInList(apple);
            //Draw a line from the player to the apple.
            Debug.DrawLine(transform.position, inputPoint, Color.red);
            //return true.
            return true;
        }
        //return false.
        return false;
    }

    //Insert apple gameobject in the list
    private void InsertAppleInList(GameObject apple)
    {
        appleList.Add(apple);
    }

    private void NearestApple()
    {
        
        //Nearest Apple distance variable.
        float nearest = 1000000000f;
        //Nearest Apple position variable.
        Vector3 nearestPosition = this.transform.position;
        //Check in list the nearest apple
        foreach (GameObject apple in appleList)
        {
            //Debug.Log(apple.name);
            //Check if the object in list is not null
            if (apple != null)
            {
                //Distance variable between player and apple.
                float distance = Vector3.Distance(apple.transform.position, this.transform.position);
                //Debug.Log(nearest);
                //Check if Distance from the apple is lesser than the previous apple
                if (distance < nearest)
                {
                    nearest = distance;
                    nearestPosition = apple.transform.position;
                    
                }
            }
        }
        
        MoveToApple(nearestPosition);
    }

    private void MoveToApple(Vector3 positionToMove)
    {
        positionToMove = new Vector3(positionToMove.x, this.transform.position.y, positionToMove.z);
        transform.position = Vector3.MoveTowards(this.transform.position, positionToMove, 1 * Time.deltaTime);
    }

    private IEnumerator StartFinding()
    {
        yield return new WaitForSeconds(5);
        NearestApple();
    }
}
