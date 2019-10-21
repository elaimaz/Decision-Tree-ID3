using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCycle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] apple;
    private float time = 0;
    private int appleCount = 0;

    //private float timeDebug = 0;
    void Start()
    {
        apple[0].SetActive(true);
        appleChange();
    }

    /*private void Update()
    {
        timeDebug -= Time.deltaTime;
        Debug.Log(timeDebug);
    }*/

    private void appleChange()
    {
        time = Random.Range(10.0f, 20.0f);
        //timeDebug = time;
        StartCoroutine(AppleChangeTime());
    }

    private IEnumerator AppleChangeTime()
    {
        yield return new WaitForSeconds(time);
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
