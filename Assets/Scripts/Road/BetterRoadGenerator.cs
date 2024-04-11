using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundObject
{
    public GameObject ground;
    public GameObject underground;

    public GroundObject(GameObject ground, GameObject underground)
    {
        this.ground = ground;
        this.underground = underground;
    }
}

[System.Serializable]
public class CoinObject
{
    public GameObject coinPrefab;
    [Range(0.0f, 1.0f)] public float spawnProbability;

    public CoinObject(GameObject coinPrefab, float spawnProbability)
    {
        this.coinPrefab = coinPrefab;
        this.spawnProbability = spawnProbability;
    }
}

public class BetterRoadGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject startUndergroundPrefab;
    [SerializeField] private GroundObject[] groundObjects;
    
    [Header("Player and Barrier")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform barrier;
    
    [Header("Spawn Settings")]
    [SerializeField] private float spawnDistance = 40f;
    [SerializeField] private float despawnDistance = 80f;
    [SerializeField] private float barrierDistance = 40f;
    
    [Header("Fuel Spawner System")]
    [SerializeField] private float fuelSpawnDistance = 100f;
    [SerializeField] private GameObject fuelPrefab;
    
    [Header("Coin Spawner System")]
    [SerializeField] private float coinSpawnDistance = 40f;
    [SerializeField] private CoinObject[] coinObjects;
    
    private float _lastBarrierPosition = 0f;
    private float _nextFuelPosition = 0f;
    private float _nextCoinsPosition = 0f;
    private List<GroundObject> _activeGrounds = new List<GroundObject>();
    private List<GameObject> _activeFuels = new List<GameObject>();
    private List<GameObject> _activeCoins = new List<GameObject>();
    void Start()
    {
        _nextFuelPosition = player.position.x + fuelSpawnDistance;
        _nextCoinsPosition = player.position.x + coinSpawnDistance;
        barrier.position = new Vector3(player.position.x - barrierDistance, barrier.position.y, 0);
        _lastBarrierPosition = barrier.position.x;
        SpawnStart();
        //player.transform.position = new Vector3(_activeGrounds[0].ground.GetComponent<SpriteRenderer>().bounds.size.x, _activeGrounds[0].ground.GetComponent<SpriteRenderer>().bounds.size.y - 4f, player.transform.position.z);
    }

    void Update()
    {
        //Fuel
        if (player.position.x > _nextFuelPosition)
        {
            _nextFuelPosition = player.position.x + fuelSpawnDistance;
            // spawn fuel
            GameObject currentGround = _activeGrounds[_activeGrounds.Count - 1].ground;
            Transform fuelSpawner = currentGround.transform.Find("FuelSpawner");
            GameObject fuel = Instantiate(fuelPrefab, transform);
            
            fuel.transform.position = fuelSpawner.transform.position;
            player.GetComponent<FuelSystem>().SetNextFuelPosition(fuel.transform.position);
            fuelSpawner.gameObject.SetActive(false);
            _activeFuels.Add(fuel);
        }
        //Coins
        if (player.position.x > _nextCoinsPosition)
        {
            _nextCoinsPosition = player.position.x + coinSpawnDistance;
            // spawn fuel
            GameObject currentGround = _activeGrounds[_activeGrounds.Count - 1].ground;
            Transform coinSpawnerParent = currentGround.transform.Find("CoinSpawners");

            // Check if coinSpawnerParent is found
            if (coinSpawnerParent != null)
            {
                // Iterate through each child of coinSpawnerParent
                for (int i = 0; i < coinSpawnerParent.childCount; i++)
                {
                    Transform child = coinSpawnerParent.GetChild(i);

                    // Generate a random value between 0 and 1
                    float randomValue = Random.value;

                    // Iterate through each coinObject and subtract its spawn probability from the random value
                    foreach (CoinObject coinObject in coinObjects)
                    {
                        randomValue -= coinObject.spawnProbability;

                        // If the random value is less than or equal to 0, spawn this coinObject
                        if (randomValue <= 0f)
                        {
                            GameObject coin = Instantiate(coinObject.coinPrefab, child.position, Quaternion.identity);
                            // Deactivate the child
                            child.gameObject.SetActive(false);
                            _activeCoins.Add(coin);
                            // Break out of the loop once a coin is spawned
                            break;
                        }
                    }
                }
            }
        }

        
        //Barrier
        if (player.position.x - barrierDistance > _lastBarrierPosition)
        {
            barrier.position = new Vector3(player.position.x - barrierDistance, barrier.position.y, 0);
            _lastBarrierPosition = barrier.position.x;
        }
        if (player.position.x + spawnDistance > _activeGrounds[_activeGrounds.Count - 1].ground.transform.position.x)
        {
            SpawnGround();
        }

        if (player.position.x - despawnDistance > _activeGrounds[0].ground.transform.position.x)
        {
            DespawnGround();
        }
    }

    // Spawn the start grounds and add them to the list
    void SpawnStart()
    {
        Vector3 spawnPosition = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            GameObject ground = Instantiate(startPrefab, transform);
            GameObject underground = Instantiate(startUndergroundPrefab, transform);
        
            float offsetX = ground.GetComponent<SpriteRenderer>().bounds.size.x * i;
            ground.transform.position = spawnPosition + new Vector3(offsetX, 0, 0);
            underground.transform.position = spawnPosition + new Vector3(offsetX, -ground.GetComponent<SpriteRenderer>().bounds.size.y + 2f, 0);
        
            _activeGrounds.Add(new GroundObject(ground, underground));
        }
    }

    // Spawn a random ground and add it to the list
    void SpawnGround()
    {
        GroundObject newTerrainPart = groundObjects[Random.Range(0, groundObjects.Length)];
        GameObject ground = Instantiate(newTerrainPart.ground, transform);
        GameObject underground = null;
    
        Vector3 spawnPosition = Vector3.zero;
        if (_activeGrounds.Count > 0)
        {
            // Berechne die Spawnposition basierend auf dem Ende des vorherigen Straßenteils
            spawnPosition = _activeGrounds[_activeGrounds.Count - 1].ground.transform.position + new Vector3(_activeGrounds[_activeGrounds.Count - 1].ground.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
        }
        
        ground.transform.position = spawnPosition;
        if (newTerrainPart.underground != null)
        {
            underground = Instantiate(newTerrainPart.underground, transform);
            underground.transform.position = spawnPosition - new Vector3(0, newTerrainPart.ground.GetComponent<SpriteRenderer>().bounds.size.y - 2f, 0);
        }
        
        _activeGrounds.Add(new GroundObject(ground, underground));
    }

    void DespawnGround()
    {
        // Destroy and Remove grounds
        Destroy(_activeGrounds[0].ground);
        Destroy(_activeGrounds[0].underground);
        _activeGrounds.RemoveAt(0);
        
        // Destroy and Remove Fuels
        /*if (_activeFuels.Count > 0)
        {
            Destroy(_activeFuels[0]);
            _activeFuels.RemoveAt(0);
        }*/
        
        // Destroy and Remove Coins
        /*if (_activeCoins.Count > 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                Destroy(_activeCoins[_activeCoins.Count - i]);
                _activeCoins.RemoveAt(_activeCoins.Count - i);
            }
        }*/
    }
}
