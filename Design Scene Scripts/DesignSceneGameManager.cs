using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DesignSceneGameManager : MonoBehaviour {

    GameObject[] panels;
    public List<GameObject> AllScenes;

    public Text Header;

    public GameObject SceneList; // For creating initial scene
    public GameObject SceneButtonInstance; // For creating initial scene

    // TempObjectHolder is used to temperarily hold a gameobject that is being initiated.
    GameObject TempObjectHolder = null;
    public GameObject GetTempObjectHolder()
    {
        return TempObjectHolder;
    }
    public void SetTempObjectHolder(GameObject gameobject)
    {
        TempObjectHolder = gameobject;
    }
    public void ResetTempObjectHolder()
    {
        TempObjectHolder = null;
    }

    bool IsExistingObject = false;
    public bool GetIsExistingObejct()
    {
        return IsExistingObject;
    }
    public void SetIsExistingObject(bool tf)
    {
        IsExistingObject = tf;
    }


    // CurrentScene holds the in-game scene we are currently looking at (designing). This is
    // initiated in the Start() function.
    GameObject CurrentScene = null;
    public void SetCurrentScene(GameObject currentscene)
    {
        CurrentScene = currentscene;
    }
    public GameObject GetCurrentScene()
    {
        return CurrentScene;
    }

    // Store last clicked button
    GameObject LastClickedButton = null;
    public GameObject GetLastClickedButton()
    {
        return LastClickedButton;
    }
    public void SetLastClickedButton(GameObject button)
    {
        LastClickedButton = button;
    }


    private void Start()
    {
        // At the start of the scene, in terms of the UI, make sure only the design panel is displayed
        panels = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject panel in panels)
        {
            if (panel.name == "Design Panel")
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }

        // At the start of the scene, store all 'Sub Panel's
        //SubPanels = GameObject.FindGameObjectsWithTag("Sub Panel");

        // At the start of the scene, store all 'Input Field's
        //InputFields = GameObject.FindGameObjectsWithTag("Input Field");

        // Initiate a default start scene and create a button in scene list.
        GameObject InitialScene = new GameObject();
        InitialScene.name = "Unnamed scene";
        InitialScene.AddComponent<AssociatedButton>();
        InitialScene.AddComponent<SceneInfo>();
        InitialScene.GetComponent<SceneInfo>().SimulationTime = 0;
        InitialScene.GetComponent<SceneInfo>().TimeStep = 0;
        InitialScene.GetComponent<SceneInfo>().GridSize = 0;
        InitialScene.tag = "Scene";
        AllScenes.Add(InitialScene);

        GameObject NewSceneButton = Instantiate(SceneButtonInstance);
        NewSceneButton.SetActive(true);
        NewSceneButton.transform.SetParent(SceneList.transform);
        NewSceneButton.GetComponentInChildren<Text>().text = InitialScene.name;
        NewSceneButton.transform.position = SceneButtonInstance.transform.position;
        NewSceneButton.transform.localEulerAngles = SceneButtonInstance.transform.localEulerAngles;
        NewSceneButton.transform.localScale = SceneButtonInstance.transform.localScale;
        NewSceneButton.GetComponent<SceneButton>().LinkedGameObject = InitialScene;

        InitialScene.GetComponent<AssociatedButton>().button = NewSceneButton;

        // Then set the CurrentScene to be this initial scene
        CurrentScene = InitialScene;

        // Setup the header
        Header.text = CurrentScene.name;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().enabled = false;
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().enabled = true;
        }
    }
}
