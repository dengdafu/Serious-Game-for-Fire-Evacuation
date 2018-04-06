using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {

    public Dropdown SceneDropDown;
    public GameObject OpenPanel;
    public GameObject ClosePanel;

    public void OnClick()
    {
        // Activate the "OpenPanel" (Build Panel)
        OpenPanel.SetActive(true);

        // Deactivate the "ClosePanel" (Design Panel)
        ClosePanel.SetActive(false);

        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initialize the AllSceneSimInfo with the same length as AllScenes, but filled with nulls
        List<GameObject> AllScenes = gamemanager.GetComponent<DesignSceneGameManager>().AllScenes;
        List<DesignSceneGameManager.SceneSimInfo> AllSceneSimInfo = new List<DesignSceneGameManager.SceneSimInfo>(AllScenes.Count);
        for (int i = 0; i < AllScenes.Count; i++)
        {
            AllSceneSimInfo.Add(new DesignSceneGameManager.SceneSimInfo(false, 0, 0, 0));
        }
        gamemanager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo = AllSceneSimInfo;

        // Fill the dropdown in the build panel with all the existing scenes;
        Dropdown.OptionData NewOption;
        SceneDropDown.ClearOptions();
        for (int i = 0; i < AllScenes.Count; i++)
        {
            NewOption = new Dropdown.OptionData();
            NewOption.text = AllScenes[i].name;
            SceneDropDown.options.Add(NewOption);
        }
    }
}
