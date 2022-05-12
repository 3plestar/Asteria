using UnityEngine;

[System.Serializable]

public class SpawnerData
{
    [SerializeField] private GameObject GObject;
    [SerializeField] private Transform[] SpawnPoints;

    public GameObject gObject => GObject;
    public Transform[] spawnPoints => SpawnPoints;
}
