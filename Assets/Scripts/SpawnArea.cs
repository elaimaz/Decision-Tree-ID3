using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObject;
    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition.y = this.gameObject.transform.position.y;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnApple();
        }
    }

    private void SpawnApple()
    {
        for (int i = 0; i < 200; i++)
        {
            spawnPosition.x = Random.insideUnitSphere.x * 3.7f;
            spawnPosition.z = Random.insideUnitSphere.z * 3.7f;
            Instantiate(spawnObject, spawnPosition, Quaternion.identity);
        }
    }
}
