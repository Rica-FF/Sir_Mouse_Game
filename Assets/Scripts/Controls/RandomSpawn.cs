using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject[] Spawns = new GameObject[0];
 
    void Start()
    {
        SpawnRandomSpot();
    }

    public void SpawnRandomSpot()
    {
        if (Spawns[0] != null)
        {
            gameObject.transform.position = Spawns[Random.Range(0, 5)].transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
