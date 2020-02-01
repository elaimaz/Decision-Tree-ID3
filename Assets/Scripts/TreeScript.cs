using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    //Handler to PlayerConeView script
    private PlayerScript playerScript;
    //Controls if the tree is already in the list or not.
    public bool inList = false;
    //Handle if the player already "visited" the tree
    public bool visited = false;
    //Chosen tree to the AI move towards
    public bool chosenTree = false;

    void Start()
    {
        //Initialize playerConeView
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the Player cannot be found then return empty
        if (playerScript == null)
        {
            return;
        }
        //Check if the apple have already fall, the player is searching for apple and the GameObject isn't in list yet.
        if (playerScript.searchingTree == true)
        {
            //Call playerConeView to test the sight of the "Player"
            playerScript.TreeSight(this.gameObject);
        }
    }
}
