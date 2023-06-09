using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/** Settings.cs
 *  Minesweeper Unity - Personal Challenge 2023
 *  
 *  Controls all UI menus, and game settings for the Minesweeper Unity project.
 *  @author Zac Seales
 */

public class Settings : MonoBehaviour
{
    // all UI Menus
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject difficultyMenu;

    /* Instructions overlay */
    public GameObject menuInstructions;

    // The volume slider object
    public Slider slider;

    // bool defining whether fullscreen is active.
    private bool fullscreenMode;

    // The resume button (only active if game is active)
    public GameObject resumeButton;

    // The current active game
    public GameObject activeGame;

    /** Close all ui panels except for main menu
     *  and set initial fixed window size.
     */
    private void Awake()
    {
        // ensure game is not active
        if (activeGame.activeSelf)
        {
            activeGame.SetActive(false);
        }
        menuInstructions.SetActive(false);
        // resume button only active when game is active
        resumeButton.SetActive(false);
        setVolume();
        fullscreenMode = false;
        // show Main Menu
        openMain();
        //set window size
        Screen.SetResolution(800, 800, fullscreenMode);
    }

    // called every frame
    void Update()
    {
        // toggles menu when esc key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggleMenu();
        }
    }

    /** Set screen resolution dependent on the name of the button pressed.
     */
    public void setResolution()
    {
        //retrieve name of clicked item
        string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (level)
        {
            case "LowRes":
                Screen.SetResolution(800, 800, fullscreenMode);
                break;
            case "MidRes":
                Screen.SetResolution(1024, 800, fullscreenMode);
                break;
            case "HighRes":
                Screen.SetResolution(1280, 720, fullscreenMode);
                break;
            case "EpicRes":
                Screen.SetResolution(1920, 1080, fullscreenMode);
                break;
        }
        // if game is active, re-adjust grid size
        if (activeGame.activeSelf)
        {
            StartCoroutine(activeGame.GetComponent<GameBoard>().waitBeforeAdjust());
        }
    }

    /** Set graphics quality dependent on the button pressed.
     */
    public void setQuality()
    {
        //retrieve name of clicked item
        string level = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (level)
        {
            case "Low":
                QualitySettings.SetQualityLevel(0);
                break;
            case "Normal":
                QualitySettings.SetQualityLevel(1);
                break;
            case "High":
                QualitySettings.SetQualityLevel(2);
                break;
            case "Epic":
                QualitySettings.SetQualityLevel(3);
                break;
        }
    }

    /** Esc key opens (or closes) the menu while a game is active.
     */
    public void toggleMenu()
    {
        // close menu if open
        if (mainMenu.activeSelf && activeGame.activeSelf)
        {
            mainMenu.SetActive(false);
        } 
        else if (!mainMenu.activeSelf)
        {
            difficultyMenu.SetActive(false);
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    /** Toggle windowed screen.
     */
    public void windowed()
    {
        fullscreenMode = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    /** Toggle full screen.
     */
    public void fullScreen()
    {
        fullscreenMode = true;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        // if game is active, re-adjust grid size
        if (activeGame.activeSelf)
        {
            activeGame.GetComponent<GameBoard>().fitToScreen();
        }
    }

    /** Close all UI Panels and open Main menu.
     */
    public void openMain()
    {
        optionsMenu.SetActive(false);
        difficultyMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /** Close Main menu and open the Options menu.
     */
    public void openOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    /** Close Main menu and open Difficulty selection menu.
     */
    public void openDifficulty()
    {
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
    }

    /** Sets a default start volume and then adds a listener to the 
     *  slider so that volume is changed as slider value changes.
     */
    public void setVolume()
    {
        //set initial volume
        AudioListener.volume = 0.75f;
        // set volume slider listener
        slider.onValueChanged.AddListener((v) =>
        {
            AudioListener.volume = v;
        });
    }

    /** Initializes a new game of minesweeper.
     */
    public void newGame()
    {
        // ensure menu has resume button
        resumeButton.SetActive(true);
        // Close difficulty selection menu
        difficultyMenu.SetActive(false);
        // close current game if active
        if(activeGame.activeSelf)
        {
            StartCoroutine(waitForReset());
        }
        else
        {
            setDifficulty();
        }
        menuInstructions.SetActive(true);
    }

    private void initGameState(int cols, int rows, int mines)
    {
        // initialize mines
        activeGame.GetComponentInChildren<GameBoard>().numMines = mines;
        // initialize arrays of box states and values
        activeGame.GetComponentInChildren<GameBoard>().boxStates = new int[rows, cols];
        activeGame.GetComponentInChildren<GameBoard>().boxValues = new int[rows, cols];
        activeGame.SetActive(true);
    }

    IEnumerator waitForReset()
    {
        activeGame.GetComponent<GameBoard>().Reset();
        yield return new WaitForSeconds(0.2f);
        activeGame.SetActive(false);
        setDifficulty();
        activeGame.GetComponent<GameBoard>().newGame();
    }

    /** Sets difficulty of new game
     */
    private void setDifficulty()
    {
        //retrieve name of clicked item
        string difficulty = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (difficulty)
        {
            case "Easy":
                initGameState(9, 9, 10);
                break;
            case "Medium":
                initGameState(16, 16, 40);
                break;
            case "Hard":
                initGameState(30, 16, 99);
                break;
            case "Impossible":
                initGameState(40, 20, 300);
                break;
            case "Custom":
                Debug.Log("Not active yet.");
                break;
        }
    }

    /** Exit the Game and close window.
     */
    public void ExitGame()
    {
        // for editor
        //UnityEditor.EditorApplication.isPlaying = false;

        //for final build
        Application.Quit();
    }
}
