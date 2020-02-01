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
    private List<GameObject> appleList = new List<GameObject>();
    //List of trees the AI knows
    private List<GameObject> treeList = new List<GameObject>();
    //Handler to starvation Bar
    private StatusBar starvationBar;
    //Variable that controls if the AI is searching or not for apples.
    public bool searchingApple = false;
    //Boolean to control if the AI need to continue going after the apple.
    private bool movingToApple = false;
    //Boolean to control if the AI need to go to a tree.
    private bool movingToTree = false;
    //Nearest Apple position variable.
    private GameObject appleGameObj = null;
    //Nearest Tree position variable.
    private GameObject treeGameObj = null;
    //Variable that controls if the AI is searching or not for trees.
    public bool searchingTree = false;
    //controler to player doing action.
    private bool doingAction = false;
    //Amount of degrees to rotate
    private float rotationLeft = 360f;
    //Control if the AI made a rotation.
    private bool madeRotation = false;

    private void Start()
    {
        starvationBar = GameObject.Find("Starve Bar Image").GetComponent<StatusBar>();
    }

    private void Update()
    {
        //If health bar is below 100 the AI starts search for apples.
        if (starvationBar.health < 100 && treeList.Count == 0)
        {
            searchingTree = true;
        }
        else if (starvationBar.health < 100 && treeList.Count > 0)
        {
            searchingApple = true;
        }
        //Rotate AI.
        if (madeRotation == false && starvationBar.health < 100)
        {
            RotateAI();
        }
        //If are none apple around the player will go to the closet tree.
        if (appleList.Count <= 0 && doingAction == false && madeRotation == true && treeList.Count > 0)
        {
            doingAction = true;
            NearestTree();
            //
        }
        //If the list o apples are not empty and the player is not doing another action
        if (appleList.Count > 0 && doingAction == false && madeRotation == true)
        {
            Action();
        }
        //Move Towards the tree.
        if (movingToTree == true && movingToApple == false)
        {
            MoveToTree(treeGameObj);
            LookAtTree(treeGameObj);
        }
        //While the searching apple is true the AI will move towards the apple.
        if (movingToApple == true)
        {
            MoveToApple(appleGameObj);
            //Look at apple
            LookAtApple(appleGameObj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If it is the choosen apple then when Ai collides this changes will occur.
        if (other.tag == "Food")
        {
            AppleCycle foodScript = other.GetComponent<AppleCycle>();
            if (foodScript.chosenApple == true)
            {
                int appleGeneratedHealth = 1;
                //Green Apple.
                if (foodScript.appleCount == 0)
                {
                    appleGeneratedHealth = 15;
                    //Red apple
                }
                else if (foodScript.appleCount == 1)
                {
                    appleGeneratedHealth = 30;
                }
                //Rotting apple.
                else
                {
                    appleGeneratedHealth = 5;
                }
                //Buff Bar
                EatApple(appleGeneratedHealth);
                //AI is no longer choosing apple.
                searchingApple = false;
                //AI is no longer doing an action.
                doingAction = false;
                //Desalocate all list.
                ResetInList();
                //Player no longer moves to apple.
                movingToApple = false;
                //Rotation is needed to be done again.
                madeRotation = false;
                //Rotation is reseted.
                rotationLeft = 360f;
                Destroy(other.gameObject);
            }
            else
            {
                return;
            }

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
        //Debug.Log(appleList.Count);
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

    //Inform if the AI get sight of a tree
    public bool TreeSight(GameObject tree)
    {
        //Get the cosene of the angle between the foward vector from AI and the vector from the tree
        float cosAngle = Vector3.Dot((tree.transform.position - this.transform.position).normalized, this.transform.forward);
        //transform the angle from radians to degrees
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
        //Distance between AI and tree.
        float distance = Vector3.Distance(this.transform.position, tree.transform.position);
        //Get Handler to TreeScript.
        TreeScript treeScript = tree.GetComponent<TreeScript>();

        //If the angle is minor than the cutoff (45 degres) and the distance is minor than 50.0f then the AI is seeing the tree.
        if (angle < cutoff && distance < 100.0f)
        {
            //Change variable to stop GameObject to continue going on list.
            if (treeScript.inList == false)
            {
                treeScript.inList = true;
                //Insert tree on the list.
                InsertTreeInList(tree);
            }
            //Draw a line from the player to the apple.
            Debug.DrawLine(transform.position, tree.transform.position, Color.green);
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

    //Insert tree gameobject in the list
    private void InsertTreeInList(GameObject tree)
    {
        treeList.Add(tree);
    }

    private void NearestApple()
    {
        //Nearest Apple distance variable.
        float nearest = 1000000000f;
        AppleCycle appleCycleScript = null;
        //Give weight for the decision of what apple to choose. When 1 == rotting apple, 2 == green apple and 3 == red apple.
        int chooseWeight = 0;
        //Check in list the nearest apple
        foreach (GameObject apple in appleList)
        {
            //Check if the object in list is not null
            if (apple != null)
            {
                searchingApple = true;
                //Distance variable between player and apple.
                float distance = Vector3.Distance(apple.transform.position, this.transform.position);
                int appleCycleIndex = ChooseBetterAppleByStatus(apple);
                //Check if Distance from the apple is lesser than the previous apple
                if (distance < nearest && appleCycleIndex > chooseWeight)
                {
                    chooseWeight = appleCycleIndex;
                    nearest = distance;
                    appleGameObj = apple;
                    appleCycleScript = apple.GetComponent<AppleCycle>();
                }
                movingToApple = true;
            }
        }
        //if the script is not null the variable chosen apple became true, this will control the next behavior of the AI.
        if (appleCycleScript != null)
        {
            appleCycleScript.chosenApple = true;
        }
    }

    private void NearestTree()
    {
        //Nearest tree distance variable.
        float nearest = 1000000000f;
        TreeScript treeScript = null;
        //Check in list the nearest tree
        foreach (GameObject tree in treeList)
        {
            //Check if the object in list is not null
            if (tree != null)
            {
                //Distance variable between AI and tree.
                float distance = Vector3.Distance(tree.transform.position, this.transform.position);
                //Check if Distance from the apple is lesser than the previous apple
                if (distance < nearest)
                {
                    nearest = distance;
                    treeGameObj = tree;
                    treeScript = tree.GetComponent<TreeScript>();
                }
                movingToTree = true;
            }
        }
        //if the script is not null the variable chosen tree became true, this will control the next behavior of the AI.
        if (treeScript != null)
        {
            treeScript.chosenTree = true;
        }
    }

    //This function will give a weight to the choose of he apple
    private int ChooseBetterAppleByStatus(GameObject apple)
    {
        AppleCycle appleCycleScript = apple.GetComponent<AppleCycle>();
        if (appleCycleScript.appleCount == 0)
        {
            return 2;
        }
        else if (appleCycleScript.appleCount == 1)
        {
            return 3;
        }
        else
        {
            return 1;
        }
    }

    //Move AI to apple.
    private void MoveToApple(GameObject applePosition)
    {
        if (applePosition != null)
        {
            //Check if the AI arrived at the apple position.
            if (transform.position.x == applePosition.transform.position.x && transform.position.z == applePosition.transform.position.z)
            {
                movingToApple = false;
            }
            //Move the AI to the apple position.
            Vector3 positionToMove = new Vector3(applePosition.transform.position.x, this.transform.position.y, applePosition.transform.position.z);
            transform.position = Vector3.MoveTowards(this.transform.position, positionToMove, 1 * Time.deltaTime);
        }
        
    }

    //Move AI next to the tree.
    private void MoveToTree(GameObject treePosition)
    {
        if (treePosition != null)
        {
            //Check if the AI arrived at the apple position.
            if (transform.position.x == treePosition.transform.position.x && transform.position.z == treePosition.transform.position.z)
            {
                movingToApple = false;
            }
            //Move the AI to the apple position.
            Vector3 positionToMove = new Vector3(treePosition.transform.position.x, this.transform.position.y, treePosition.transform.position.z);
            transform.position = Vector3.MoveTowards(this.transform.position, positionToMove, 1 * Time.deltaTime);
        }

    }

    //Wait 10 seconds to start searching the nearest apple.
    private IEnumerator StartFinding()
    {
        yield return new WaitForSeconds(3);
        doingAction = true;
        NearestApple();
    }

    //Call AppleUP from statusBar script, this will buff player bar.
    public void EatApple(int appleEat)
    {
        starvationBar.AppleUp(appleEat);
    }

    //Clean List.
    private void CleanList()
    {
        Debug.Log("CleanList");
        searchingApple = false;
        appleList.Clear();
    }

    //This method will reset the variable of all the apples GameObject in the list appleList.
    private void ResetInList()
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
                //Test if the apple needed to be destroyed or no.
                appleCycle.appleChange();
            }
        }
        CleanList();
    }

    //Rotate AI in 360 degree.
    private void RotateAI()
    {
        //Degrade rotationLeft over time.
        float rotation = 20 * Time.deltaTime;
        if (rotationLeft > rotation)
        {
            rotationLeft -= rotation;
        }
        else
        {
            rotation = rotationLeft;
            rotationLeft = 0;
        }
        //Make rotation in the Y axis.
        transform.Rotate(0, rotation, 0);
        if (rotationLeft <= 0 && (appleList.Count > 0 || treeList.Count > 0))
        {
            madeRotation = true;
        }else if (rotationLeft <= 0 && (appleList.Count == 0 || treeList.Count == 0))
        {
            rotationLeft = 360;
        }
    }

    //Look to the nearest apple, make the AI face the apple.
    private void LookAtApple(GameObject appleGameObj)
    {
        if (appleGameObj != null)
        {
            Vector3 dir = appleGameObj.transform.position - transform.position;
            //Y needs to be zero to the AI not rotate in another Axis.
            dir.y = 0;
            //Check if the dir is not zero.
            if (dir != Vector3.zero)
            {
                //Realizes the rotation.
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1.5f * Time.deltaTime);
            }
        }
    }

    //Look to the nearest tree, make the AI face the tree.
    private void LookAtTree(GameObject treeGameObj)
    {
        if (treeGameObj != null)
        {
            Vector3 dir = treeGameObj.transform.position - transform.position;
            //Y needs to be zero to the AI not rotate in another Axis.
            dir.y = 0;
            //Check if the dir is not zero.
            if (dir != Vector3.zero)
            {
                //Realizes the rotation.
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1.5f * Time.deltaTime);
            }
        }
    }
}
