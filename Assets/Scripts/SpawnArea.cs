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
        spawnPosition.y = this.gameObject.transform.position.y;
        InvokeRepeating("Randomizer", 1f, 60f);
    }

    //Randomize the number o apples per season.
    private void Randomizer()
    {
        randomApples = Random.Range(0, 100);
        SpawnApple();
    }

    //Randomize spawn X and Z position and Instantiate for each apple
    private void SpawnApple()
    {
        for (int i = 0; i < randomApples; i++)
        {
            spawnPosition.x = Random.insideUnitSphere.x * 3.7f;
            spawnPosition.z = Random.insideUnitSphere.z * 3.7f;
            spawnApple = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
            spawnApple.transform.parent = this.gameObject.transform.parent;
        }
    }
}
