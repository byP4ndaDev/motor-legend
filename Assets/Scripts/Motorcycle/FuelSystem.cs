using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{
    [Header("Fuel Text")]
    [SerializeField] private TextMeshProUGUI fuelText;
    
    [Header("Fuel Settings")]
    [SerializeField] private Gradient fuelBarGradient;
    [SerializeField] private float fuelConsumption = 0.05f;
    [SerializeField] private float _currentFuel = 100;
    public float maxFuel = 100f;

    private Image _fuelBarSprite;
    private Vector3 _nextFuelPosition = Vector3.zero;

    private void Start()
    {
        // Set the next fuel positon and get the fuelbarSprite
        _nextFuelPosition = new Vector3(transform.position.x + 60f, 0, 0);
        _fuelBarSprite = GameObject.FindGameObjectWithTag("FuelBar").GetComponent<Image>();
        fuelText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Check if there is fuel
        if (_currentFuel > 0)
        {
            // reduce the current fuel and update the fuelbar
            _currentFuel -= fuelConsumption;
            UpdateFuelBar(_currentFuel);
            int distance = (int) Vector3.Distance(transform.position, _nextFuelPosition);
            // Show the distance to the next fuel
            if (distance <= 40 && distance > 0)
            {
                fuelText.gameObject.SetActive(true);
                fuelText.text = distance + "m";
            }
            else
            {
                if (distance < 1)
                {
                    SetNextFuelPosition(Vector3.zero);
                }
                fuelText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Empty fuel, end game
            transform.GetComponent<BikeController>().OnDeath();
        }
    }
    
    // Set the fuel to the maximum
    public void SetMaxFuel()
    {
        _currentFuel = maxFuel;
        UpdateFuelBar(_currentFuel);
    }

    // Update the fuelbarSprite
    private void UpdateFuelBar(float currentFuel)
    {
        _fuelBarSprite.fillAmount = currentFuel / maxFuel;
        _fuelBarSprite.color = fuelBarGradient.Evaluate(1- _fuelBarSprite.fillAmount);
    }
    
    public Vector3 GetNextFuelPosition()
    {
        return _nextFuelPosition;
    }
    
    // Update the next fuel position
    public void SetNextFuelPosition(Vector3 nextFuelPosition)
    {
        this._nextFuelPosition = nextFuelPosition;
        Debug.Log(nextFuelPosition);
    }
}
