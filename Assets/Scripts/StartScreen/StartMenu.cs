using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    // Load the game scene
    public void StartGame()
    {
        SceneManager.LoadScene("GreenHillsLevel", LoadSceneMode.Single);
    }
    
    public void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    
    public void HideSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }
    
    
}
