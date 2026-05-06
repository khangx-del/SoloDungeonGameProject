using System.Collections;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class logicManager : MonoBehaviour
{

    private GameObject pauseMenu;
    private GameObject MenuButton;
    private GameObject panel;
    private GameObject statbutton;
    private GameObject exitStatButtons;
    private GameObject CurrentLevel;
    private GameObject SkillpointUI;
    

    public void Awake()
    {
        CurrentLevel = GameObject.Find("CurrentLevel");
        if (CurrentLevel != null)
        {
            CurrentLevel.SetActive(false); 
        }
        pauseMenu = GameObject.Find("Menu"); 
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false); // Ensure the pause menu is hidden when the game starts
        }
        MenuButton = GameObject.Find("MenuButton");
        if (MenuButton != null)
        {
            MenuButton.SetActive(true); // Ensure the menu button is visible when the game starts
        }
          
        statbutton = GameObject.Find("stats");
        if (statbutton != null)
        {
            statbutton.SetActive(true); 
        }
        
        
        exitStatButtons = GameObject.Find("exitstatsbutton");
        if (exitStatButtons != null)
        {
            exitStatButtons.SetActive(false); 
        }

        panel = GameObject.Find("LevelUpPanel");
        if (panel != null)        
        {
            panel.SetActive(false); // Ensure the level up panel is hidden when the game starts
        }
        SkillpointUI = GameObject.Find("SkillPointUI");
        if (SkillpointUI != null)        
        {
           SkillpointUI.SetActive(false); // Ensure the skill point UI is hidden when the game starts
        }
        
    }
    public void Start()
    {
        StartCoroutine(ShowCurrentLevel());
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        pauseMenu.SetActive(true); // Show the pause menu
        MenuButton.SetActive(false); // Hide the menu button when the game is paused
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        pauseMenu.SetActive(false); // Hide the pause menu
        MenuButton.SetActive(true); // Show the menu button when the game is resumed
    }
   
    public void Exit()
    {
        Application.Quit(); // Quit the application
    } 
    
    public void Restart()
    {
        GameData.Reset(); // Reset game data to initial values
        SceneManager.LoadScene(0);
    }
    public void ShowLevelUpPanel()
    {
        SkillpointUI.SetActive(true); // Show the skill point UI when the level up panel is active
        statbutton.SetActive(false); // Hide the stat buttons when the level up panel is active
        panel.SetActive(true); // Show the level up panel
        Time.timeScale = 0f; // Pause the game while the level up panel is active
        exitStatButtons.SetActive(true); // Show the exit and stat buttons when the level up panel is active
    }
    public void HideLevelUpPanel()
    {
        SkillpointUI.SetActive(false); // Hide the skill point UI when the level up panel is hidden
        statbutton.SetActive(true); // Show the stat buttons when the level up panel is hidden
        panel.SetActive(false); // Hide the level up panel
        Time.timeScale = 1f; // Resume the game when the level up panel is hidden
        exitStatButtons.SetActive(false); // Hide the exit and stat buttons when the level up panel is hidden
    }
    private IEnumerator ShowCurrentLevel()
    {     
        CurrentLevel.SetActive(true); // Show the current level text
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        CurrentLevel.SetActive(false); // Hide the current level text    
    }
}
