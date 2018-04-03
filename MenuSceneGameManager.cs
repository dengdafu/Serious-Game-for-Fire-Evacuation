using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneGameManager : MonoBehaviour {

    public Slider AudioVolumeSlider;
    public AudioSource AudioSource;

    private void Start()
    {
        // Make sure all panels (popup windows) are disabled at the start of the game
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        // The following line adjusts the audio volume acccording to the change on audio slider
        AudioSource.volume = AudioVolumeSlider.value;
    }

    public void MenuSceneChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void MenuSceneQuitGame()
    {
        Application.Quit();
    }

    public void MenuSceneOpenPanel(GameObject panel)
    {
        if (!panel.activeInHierarchy)
        {
            panel.SetActive(true);
        }
    }

    public void MenuSceneClosePanel(GameObject panel)
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
        }
    }
}
