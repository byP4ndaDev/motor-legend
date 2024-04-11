using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject undergroundPrefab;
    [SerializeField] private GameObject[] groundPrefabs;
    
    [Header("Player and Barrier")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform barrier;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 40f;
    [SerializeField] private float despawnDistance = 80f;
    [SerializeField] private float barrierDistance = 40f;
    
    [Header("Fuel Spawner System")]
    [SerializeField] private float fuelSpawnDistance = 100f;
    
    private float lastBarrierPosition = 0f;
    private float nextFuelPosition = 0f;
    private List<GameObject> activeGrounds = new List<GameObject>();
    private List<GameObject> activeUndergrounds = new List<GameObject>();

    void Start()
    {
        nextFuelPosition = player.position.x + fuelSpawnDistance;
        barrier.position = new Vector3(player.position.x - barrierDistance, barrier.position.y, 0);
        lastBarrierPosition = barrier.position.x;
        SpawnStart();
    }

    void Update()
    {
        //Fuel
        if (player.position.x > nextFuelPosition)
        {
            nextFuelPosition = player.position.x + fuelSpawnDistance;
            // spawn fuel
        }
        
        //Barrier
        if (player.position.x - barrierDistance > lastBarrierPosition)
        {
            barrier.position = new Vector3(player.position.x - barrierDistance, barrier.position.y, 0);
            lastBarrierPosition = barrier.position.x;
        }
        if (player.position.x + spawnDistance > activeGrounds[activeGrounds.Count - 1].transform.position.x)
        {
            SpawnGround();
        }

        if (player.position.x - despawnDistance > activeGrounds[0].transform.position.x)
        {
            DespawnGround();
        }
    }

    void SpawnStart()
    {
        
        GameObject firstGround = Instantiate(startPrefab, transform);
        GameObject secondGround = Instantiate(startPrefab, transform);
        GameObject spawnGround = Instantiate(startPrefab, transform);
        
        GameObject firstUnderground = Instantiate(undergroundPrefab, transform);
        GameObject secondUnderground = Instantiate(undergroundPrefab, transform);
        GameObject spawnUnderground = Instantiate(undergroundPrefab, transform);
        
        Vector3 spawnPosition = Vector3.zero;
        spawnGround.transform.position = spawnPosition;
        firstGround.transform.position = spawnPosition - new Vector3(spawnGround.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
        secondGround.transform.position = spawnPosition - new Vector3(spawnGround.GetComponent<SpriteRenderer>().bounds.size.x * 2, 0, 0);
        
        spawnUnderground.transform.position = spawnPosition - new Vector3(0, spawnGround.GetComponent<SpriteRenderer>().bounds.size.y, 0);
        firstUnderground.transform.position = spawnPosition - new Vector3(spawnGround.GetComponent<SpriteRenderer>().bounds.size.x, spawnGround.GetComponent<SpriteRenderer>().bounds.size.y -2f, 0);
        secondUnderground.transform.position = spawnPosition - new Vector3(spawnGround.GetComponent<SpriteRenderer>().bounds.size.x * 2, spawnGround.GetComponent<SpriteRenderer>().bounds.size.y -2f, 0);
        
        activeGrounds.Add(firstGround);
        activeGrounds.Add(secondGround);
        activeGrounds.Add(spawnGround);
        
        activeUndergrounds.Add(firstUnderground);
        activeUndergrounds.Add(secondUnderground);
        activeUndergrounds.Add(spawnUnderground);
    }

    void SpawnGround()
    {
        
        GameObject newGround = Instantiate(groundPrefabs[Random.Range(0, groundPrefabs.Length)], transform);
        GameObject newUnderground = Instantiate(undergroundPrefab, transform);
        
        Vector3 spawnPosition = Vector3.zero;
        if (activeGrounds.Count > 0)
        {
            spawnPosition = activeGrounds[activeGrounds.Count - 1].transform.position + new Vector3(newGround.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
        }
        newGround.transform.position = spawnPosition;
        newUnderground.transform.position = spawnPosition - new Vector3(newGround.GetComponent<SpriteRenderer>().bounds.size.x, newGround.GetComponent<SpriteRenderer>().bounds.size.y -2f, 0);;
        
        activeGrounds.Add(newGround);
        activeUndergrounds.Add(newUnderground);
    }

    void DespawnGround()
    {
        Destroy(activeGrounds[0]);
        activeGrounds.RemoveAt(0);
        Destroy(activeUndergrounds[0]);
        activeUndergrounds.RemoveAt(0);
    }
}
