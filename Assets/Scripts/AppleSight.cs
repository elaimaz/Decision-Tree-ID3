using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSight : MonoBehaviour
{
    //Handler to PlayerConeView script
    private PlayerConeView playerConeView;
    //Handler to Player tranform
    private Transform playerTransform;

    //Initialize playerConeView and playerTransform.
    private void Start()
    {
        //Initialize playerConeView
        playerConeView = GameObject.FindWithTag("Player").GetComponent<PlayerConeView>();
        //initializing playerTransform
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    //Draw Gizmoz on editor
    private void OnDrawGizmos()
    {
        //If the Player cannot be found then return empty
        if (playerConeView == null)
        {
            return;
        }
        //Distance between player and apple.
        float distance = Vector3.Distance(this.transform.position, playerTransform.position);
        Debug.Log(distance);
        //Call playerConeView to test the sight of the "Player"
        playerConeView.Sight(this.transform.position);
    }
}
