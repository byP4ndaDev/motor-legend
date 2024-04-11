using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int coinValue = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the coin
        if (other.transform.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddCoin(coinValue);
            Destroy(gameObject);
        }
    }
}
