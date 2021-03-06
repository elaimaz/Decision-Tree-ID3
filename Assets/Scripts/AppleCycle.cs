﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCycle : MonoBehaviour
{
    //Apple objects, 0 green, 1 normal, 2 rotting.
    [SerializeField]
    private GameObject[] apple;
    //Apple phase time control
    private float time = 0;
    //What apple is used, 0 green, 1 normal, 2 rotting.
    public int appleCount = 0;
    //Rigidbody to use gravity.
    private Rigidbody rb;
    //Sphere Collider handler
    private SphereCollider sphereCollider;
    //the probability of the apple fall.
    private float fallChance = 0;
    //Fall Status of an apple, false = yet in the tree and true = for fall
    private bool fallStatus = false;
    //Handler to PlayerConeView script
    private PlayerScript playerScript;
    //Controls if the apple is already in the list or not.
    public bool inList = false;
    //Check if it is the apple chosen by the AI.
    public bool chosenApple = false;

    //Active first apple, call appleChange() and atribute Rigidbody to rb.
    void Start()
    {
        //Initialize playerConeView
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        //Active green apple
        apple[0].SetActive(true);
        appleChange();
        rb = gameObject.GetComponent<Rigidbody>();
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        InvokeRepeating("randomFall", 0f, 1f);
    }

    private void Update()
    {
        //If the Player cannot be found then return empty
        if (playerScript == null)
        {
            return;
        }
        //Check if the apple have already fall, the player is searching for apple and the GameObject isn't in list yet.
        if (fallStatus == true && playerScript.searchingApple == true)
        {
            //Call playerConeView to test the sight of the "Player"
            playerScript.Sight(this.transform.position, this.gameObject);
        }
    }

    //randomize phase time of the apple and call AppleChangeTime().
    public void appleChange()
    {
        time = Random.Range(10.0f, 20.0f);
        StartCoroutine(AppleChangeTime());
    }

    //Wait time to change apple lifephase.
    private IEnumerator AppleChangeTime()
    {
        yield return new WaitForSeconds(time);
        if (inList == false)
        {
            if (appleCount < 2)
            {
                Destroy(apple[appleCount]);
                appleCount++;
                apple[appleCount].SetActive(true);
                appleChange();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    //Calculate the change of a apple fall from the tree. 1% of chance when green, 10% when red, 20% when rotting, this happens every second.
    private void randomFall()
    {
        //Give a random number to fallchance, from 0 to 100.
        fallChance = Random.Range(0.0f, 100.0f);
        if (appleCount == 0 && fallChance >= 99.0f)
        {
            //Activate apple rigidbody.
            rb.useGravity = true;
            //Set fall status to true.
            fallStatus = true;
            //Activate collider.
            sphereCollider.enabled = true;
        }else if (appleCount == 1 && fallChance >= 90)
        {
            rb.useGravity = true;
            fallStatus = true;
            sphereCollider.enabled = true;
        }
        else if (appleCount == 2 && fallChance >= 80)
        {
            rb.useGravity = true;
            fallStatus = true;
            sphereCollider.enabled = true;
        }
    }
}
