using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{

    public event Action<Collider2D> EnteredTrigger; 
    public event Action<Collider2D> ExitedTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnteredTrigger?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ExitedTrigger?.Invoke(other);
    }
}
