using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Settings : MonoBehaviour
{
    // all UI Menus
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject difficultyMenu;

    // The volume slider object
    public Slider slider;

    // bool defining whether fullscreen is active.
    private bool fullscreenMode;

    /** Close all ui panels except for main menu
     *  and set initial fixed window size.
     */
    private void Awake()
    {
        setVolume();
        fullscreenMode = false;
        // show Main Menu
        openMain();
        //set window size
        Screen.SetResolution(600, 800, fullscreenMode);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Screen.SetResolution(640, 480, fullscreenMode);
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
