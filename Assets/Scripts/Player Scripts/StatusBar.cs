using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [Header("Type of Bar")]
    //Choose bar name.
    [SerializeField]
    private string barName;
    //Get component image for this object
    private Image barImage;
    //Total Health
    private float health = 100;
    
    //Rate of dropping
    [Header("Time Rate of Bar")]
    [SerializeField]
    private float dropRate = 10;
    //Time drop rate
    [SerializeField]
    private float time = 10;
    
    //Player Script reference
    private PlayerScript player;

    //Get image reference.
    private void Start()
    {
        barImage = gameObject.GetComponent<Image>();
        player = GameObject.Find("Player Prototype").GetComponent<PlayerScript>();
        InvokeRepeating("DownBar", time, time);
    }

    //Subtract bar points, change bar image.
    private void DownBar()
    {
        health -= dropRate;
        barImage.fillAmount = health / 100f;

        if (health <= 50)
        {
            barImage.color = Color.yellow;
            player.StatusUpdate(barName, 1);
        }
        if (health <= 20)
        {
            barImage.color = Color.red;
            player.StatusUpdate(barName, 2);
        }
    }
}
