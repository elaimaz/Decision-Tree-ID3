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
    //Handler to starvation Bar
    private StatusBar starvationBar;
    //Variable that controls if the AI is searching or not for apples.
    public bool searchingApple = false;
    //Boolean to control if the AI need to continue going after the apple.
    private bool movingToApple = false;
    //Nearest Apple position variable.
    private Vector3 nearestPosition;

    private void Start()
    {
        starvationBar = GameObject.Find("Starve Bar Image").GetComponent<StatusBar>();
        Action();
    }

    private void Update()
    {
        //While the searching apple is true the AI will move towards the apple.
        if (movingToApple == true)
        {
            MoveToApple(nearestPosition);
        }
    }

    //This method will decide the decision of the AI.
    public void Action()
    {
        Debug.Log("Action");
        StartCoroutine(StartFinding());
    }

    //Update bar situation to player
    public void StatusUpdate(string barName, int status, float starvationValue)
    {
        if (barName == "Starve")
        {
            starvationLevel = status;
            //Check if AI died.
            if (starvationValue < 0)
            {
                Debug.Log("Died of starvation!");
            }
        }
    }

    //Return true if he can see an apple in his front
    public bool Sight(Vector3 inputPoint, GameObject apple)
    {
        Debug.Log(appleList.Count);
        //Get the cosene of the angle between the foward vector from player and the vector from the apple
        float cosAngle = Vector3.Dot((inputPoint - this.transform.position).normalized, this.transform.forward);
        //transform the angle from radians to degrees
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
        //Distance between player and apple.
        float distance = Vector3.Distance(this.transform.position, apple.transform.position);
        //Get Handler to AppleCycle.
        AppleCycle appleCycle = apple.GetComponent<AppleCycle>();

        //If the angle is minor than the cutoff (45 degres) and the distance is minor than 15.0f then the "Player" is seeing the apple.
        if (angle < cutoff && distance < 15.0f)
        {
            //Change variable to stop GameObject to continue going on list.
            if (appleCycle.inList == false)
            {
                appleCycle.inList = true;
                //Insert apple on the list.
                InsertAppleInList(apple);
            }
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
        //Nearest position is initialiaze in the AI position.
        nearestPosition = this.transform.position;
        //Check in list the nearest apple
        foreach (GameObject apple in appleList)
        {
            //Check if the object in list is not null
            if (apple != null)
            {
                //Distance variable between player and apple.
                float distance = Vector3.Distance(apple.transform.position, this.transform.position);
                //Check if Distance from the apple is lesser than the previous apple
                if (distance < nearest)
                {
                    nearest = distance;
                    nearestPosition = apple.transform.position;
                    
                }
                movingToApple = true;
            }
        }
    }

    private void MoveToApple(Vector3 positionToMove)
    {
        //Check if the AI arrived at the apple position.
        if (transform.position.x == positionToMove.x && transform.position.z == positionToMove.z)
        {
            movingToApple = false;
            //Call action for AI perform another action.
            Action();
        }
        //Move the AI to the apple position.
        positionToMove = new Vector3(positionToMove.x, this.transform.position.y, positionToMove.z);
        transform.position = Vector3.MoveTowards(this.transform.position, positionToMove, 1 * Time.deltaTime);
    }

    //Wait 10 seconds to start searching the nearest apple.
    private IEnumerator StartFinding()
    {
        yield return new WaitForSeconds(10);
        searchingApple = true;
        NearestApple();
    }

    //Call AppleUP from statusBar script, this will buff player bar.
    public void EatApple(int appleEat)
    {
        starvationBar.AppleUp(appleEat);
    }

    private void CleanList()
    {
        Debug.Log("CleanList");
        appleList.Clear();
    }

    //This method will reset the variable of all the apples GameObject in the list appleList.
    public void ResetInList()
    {
        Debug.Log("ResetInList");
        foreach (GameObject apple in appleList)
        {
            //Check if the variable isn't null
            if (apple != null)
            {
                //Handler to AppleCycle script
                AppleCycle appleCycle = apple.GetComponent<AppleCycle>();
                appleCycle.inList = false;
            }
        }
        CleanList();
    }
}
