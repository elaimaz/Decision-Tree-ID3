using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObject;
    private Vector3 spawnPosition;
    private GameObject spawnApple;
    private int randomApples;

    private void Start()
    {
        spawnPosition.y = this.gameObject.transform.position.y;
        Randomizer();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnApple();
        }
    }

    private void Randomizer()
    {
        randomApples = Random.Range(0, 5);
        Debug.Log(randomApples);
    }

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
