using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsMenu;
    private bool isOpen = false;

    private void Start()
    {
        settingsMenu.SetActive(false);
    }
}
