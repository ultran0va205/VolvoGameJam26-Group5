using UnityEngine;
using System.Collections.Generic;

public class ConveyorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject metalPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    private void SpawnItem()
    {
        Instantiate(metalPrefab, spawnPoint.position, Quaternion.identity);
    }
}
