using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class BetterTerrainGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    //[SerializeField] private GameObject[] connectionPrefabs;
    [SerializeField] private GameObject spriteShapePrefab;
    
    [Header("Spawn Settings")]
    //[SerializeField] private float spawnDistance = 40f;
    //[SerializeField] private float despawnDistance = 80f;
    
    [Header("Terrain Length")]
    [SerializeField, Range(20, 500)] private int minLevelLength = 20;
    [SerializeField, Range(20, 500)] private int maxLevelLength = 100;
    
    [Header("Terrain X Multiplier")]
    [SerializeField, Range (5f, 7f)] private float minXMultiplier = 5f;
    [SerializeField, Range (5f, 7f)] private float maxXMultiplier = 7f;
    
    [Header("Terrain Y Multiplier")]
    [SerializeField, Range(7f, 15f)] private float minYMultiplier = 7f;
    [SerializeField, Range(7f, 15f)] private float maxYMultiplier = 15f;
    
    [Header("Terrain Curves")]
    [SerializeField, Range(0.2f, 0.4f)] private float minCurveSmoothness = 0.2f;
    [SerializeField, Range(0.2f, 0.4f)] private float maxCurveSmoothness = 0.4f;
    
    [Header("Terrain Noise Steps")]
    [SerializeField] private float minNoiseStep = 0.5f;
    [SerializeField] private float maxNoiseStep = 200f;
    
    private Vector3 _lastPos;
    private List<GameObject> _spriteShapes = new();
    private GameObject _player;
    private float _bottom = 15f;
    
    private Vector3 nextSpawnPosition;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        GenerateTerrain(new Vector3(-10, 0, 0));
        GameObject lastSpriteShape = _spriteShapes[_spriteShapes.Count - 1];
        SpriteShapeController lastSpriteShapeController = lastSpriteShape.GetComponent<SpriteShapeController>();
        nextSpawnPosition = lastSpriteShapeController.spline.GetPosition(lastSpriteShapeController.spline.GetPointCount() - 1);
    }

    private void FixedUpdate()
    {
        /*if (_spriteShapes.Count > 0)
        {
            GameObject lastSpriteShape = _spriteShapes[_spriteShapes.Count - 1];
            SpriteShapeController lastSpriteShapeController = lastSpriteShape.GetComponent<SpriteShapeController>();
            Vector3 lastSpriteShapeEndPosition = lastSpriteShapeController.spline.GetPosition(lastSpriteShapeController.spline.GetPointCount() - 1);
            
            if (_player.transform.position.x > lastSpriteShapeEndPosition.x - spawnDistance)
            {
                GenerateTerrain(new Vector3(lastSpriteShapeEndPosition.x, 0, 0));
            }
        }*/
    }

    private void GenerateTerrain(Vector3 startPosition)
    {
        
        int levelLength = Random.Range(minLevelLength, maxLevelLength);
        float xMultiplier = Random.Range(minXMultiplier, maxXMultiplier);
        float yMultiplier = Random.Range(minYMultiplier, maxYMultiplier);
        float curveSmoothness = Random.Range(minCurveSmoothness, maxCurveSmoothness);
        float noiseStep = Random.Range(minNoiseStep, maxNoiseStep);
        
        GameObject spriteShape = Instantiate(spriteShapePrefab, transform);
        spriteShape.transform.position = startPosition;
        
        SpriteShapeController spriteShapeController = spriteShape.GetComponent<SpriteShapeController>();
        
        spriteShapeController.spline.Clear();
        for (int i = 0; i < levelLength; i++)
        {
            _lastPos = transform.position + new Vector3(i * xMultiplier, Mathf.PerlinNoise(0, i * noiseStep) * yMultiplier);
            spriteShapeController.spline.InsertPointAt(i, _lastPos);

            if (i != 0 && i != levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMultiplier * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMultiplier * curveSmoothness);
            }
        }
        
        spriteShapeController.spline.InsertPointAt(levelLength, new Vector3(_lastPos.x, transform.position.y - _bottom));
        spriteShapeController.spline.InsertPointAt(levelLength + 1, new Vector3(transform.position.x, transform.position.y - _bottom));

        _spriteShapes.Add(spriteShape);
        
    }
}
