using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{ 
    [SerializeField] private SpawnerData[] spawnerData;


    public void SpawnObjects()
    {
        foreach (SpawnerData data in spawnerData)
        {
            foreach (Transform spawnpoint in data.spawnPoints)
            {
                Instantiate(data.gObject, spawnpoint.position, Quaternion.identity);
            }
        }
    }
}
