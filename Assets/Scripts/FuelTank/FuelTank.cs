using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the fuel tank
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<FuelSystem>().SetMaxFuel();
            Destroy(gameObject);
        }
    }
}
