using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    [Header("Points Text")]
    [SerializeField] private TextMeshProUGUI pointsText;
    
    [Header("Points")]
    [SerializeField] float positionMargin = 1f;
    private int points = 0;
    private float lastPosition = 0;

    private GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastPosition = player.transform.position.x;
        pointsText.text = points.ToString();
    }

    void FixedUpdate()
    {
        if (player.transform.position.x > lastPosition + positionMargin)
        {
            AddPoints(1);
            lastPosition = player.transform.position.x;
        }
    }

    public void AddPoints(int amount){
        points += amount;
        pointsText.text = points.ToString();
    }
    
    public int GetPoints(){
        return points;
    }
}
