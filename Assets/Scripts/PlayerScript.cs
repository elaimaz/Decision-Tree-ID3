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

    //Update bar situation to player
    public void StatusUpdate(string barName, int status)
    {
        if (barName == "Starve")
        {
            starvationLevel = status;
        }
    }
}
