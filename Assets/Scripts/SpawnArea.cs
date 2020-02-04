using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    //Object to be instantiate
    [SerializeField]
    private GameObject spawnObject;
    //Instantiate position
    private Vector3 spawnPosition;
    //instantiate object
    private GameObject spawnApple;
    //Number os apples per season
    private int randomApples;
    
    //Set Y spawn position and call Randomizer() at every 60 seconds.
    private void Start()
    {
        //Get the height of the spawn position.
        spawnPosition.y = this.gameObject.transform.position.y;
        //Repeat Randomizer() at every 5 minutes. 
        InvokeRepeating("Randomizer", 1f, 300f);
    }

    //Randomize the number o apples per season.
    private void Randomizer()
    {
        //set a random numer o apples to be spawned from 0 to 10.
        randomApples = Random.Range(0, 10);
        SpawnApple();
    }

    //Randomize spawn X and Z position and Instantiate for each apple
    private void SpawnApple()
    {
        //Repeat this to every apple.
        for (int i = 0; i < randomApples; i++)
        {
            //Randomize the spawn position on the X axis with a radius of a 3.7 and the position of the spawn gameobject.
            spawnPosition.x = Random.insideUnitSphere.x * 3.7f + this.transform.position.x;
            //Randomize the spawn position on the Z axis with a radius of a 3.7 and the position of the spawn gameobject.
            spawnPosition.z = Random.insideUnitSphere.z * 3.7f + this.transform.position.z;
            //Instantiate the apple at the random area near the spawn point.
            spawnApple = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            //Scale one level in parenting, apples are now child of tree.
            spawnApple.transform.parent = this.gameObject.transform.parent;
        }
    }
}
