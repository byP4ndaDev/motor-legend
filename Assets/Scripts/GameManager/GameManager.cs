using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Settings Menu")]
    [SerializeField] private GameObject settingsMenu;
    private bool isSettingsMenuOpen = false;
    
    [Header("End Screen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI scoreInRoundText;
    [SerializeField] private TextMeshProUGUI coinsInRoundText;
    
    [Header("Coin Text")]
    [SerializeField] private TextMeshProUGUI coinText;
    
    [Header("Score System")]
    [SerializeField] private PointsSystem scoreSystem;
    
    private int coins = 0;
    private int coinsInRound = 0;

    private void Start()
    {
        // Get the coins from the player prefs and set the text
        coins = PlayerPrefs.GetInt("allCoins", 0);
        coinText.text = coins.ToString();
        // Close the settings and endScreen menu by default
        settingsMenu.SetActive(false);
        endScreen.SetActive(false);
    }
    
    void Update()
    {
        // Check if the Escape key is pressed and open/close settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the settings menu is already open
            if (isSettingsMenuOpen)
            {
                isSettingsMenuOpen = false;
                settingsMenu.SetActive(false);
            }
            else
            {
                isSettingsMenuOpen = true;
                settingsMenu.SetActive(true);
            }
        }
    }

    public void AddCoin(int coinAmount)
    {
        // Adding coins and update the text
        coins += coinAmount;
        coinsInRound += coinAmount;
        coinText.text = coins.ToString();
    }

    public void EndGame()
    {
        // Set the coins in the player prefs
        PlayerPrefs.SetInt("allCoins", coins);
        int score = scoreSystem.GetPoints();
        // Get the current language and set it as a string
        scoreInRoundText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Translations", "EndScreen_ScoreText") + ": " + score;
        coinsInRoundText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Translations", "EndScreen_CoinsText") + ": " + coinsInRound;
        // Show the end screen after 2 seconds
        StartCoroutine(ShowEndScreen(2));
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene("GreenHillsLevel", LoadSceneMode.Single);
    }
    
    public void GoToMenu()
    {
        // Load the start screen
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
    }
    
    private IEnumerator ShowEndScreen(int seconds)
    {
        // Continue after 2 Seconds
        yield return new WaitForSeconds(seconds);
        // Show the end screen
        endScreen.SetActive(true);
    }
}
