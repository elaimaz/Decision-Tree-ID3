using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSight : MonoBehaviour
{
    //Handler to PlayerConeView script
    private PlayerConeView playerConeView;

    //Initialize playerConeView
    private void Start()
    {
        playerConeView = GameObject.FindWithTag("Player").GetComponent<PlayerConeView>();
    }

    //Draw Gizmoz on editor
    private void OnDrawGizmos()
    {
        //If the Player cannot be found then return empty
        if (playerConeView == null)
        {
            return;
        }

        //Call playerConeView to test the sight of the "Player"
        playerConeView.Sight(this.transform.position);
    }
}
