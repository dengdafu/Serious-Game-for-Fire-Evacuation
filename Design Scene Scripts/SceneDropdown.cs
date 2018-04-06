using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneDropdown : MonoBehaviour {

    public Dropdown dropdown;
    public GameObject RelatedPanel;
    public GameObject GameManager;
    public int CurrentIndex;

    public void OnValueChanged(int index)
    {
        CurrentIndex = index;

        List<DesignSceneGameManager.SceneSimInfo> AllSceneSimInfo = 
            GameManager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo;
        List<GameObject> AllScenes =
            GameManager.GetComponent<DesignSceneGameManager>().AllScenes;

        if (AllSceneSimInfo[index].Assigned == true)
        { 
            InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
            foreach (InputField inputfield in AllInputFields)
            {
                if (inputfield.name == "Simulation Time")
                {
                    inputfield.text = AllSceneSimInfo[index].SimulationTime.ToString();
                }
                else if (inputfield.name == "Time Step")
                {
                    inputfield.text = AllSceneSimInfo[index].TimeStep.ToString();
                }
                else if (inputfield.name == "Grid Size")
                {
                    inputfield.text = AllSceneSimInfo[index].GridSize.ToString();
                }
            }
        }
        else
        {
            InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
            foreach (InputField inputfield in AllInputFields)
            {
                if (inputfield.name == "Simulation Time")
                {
                    inputfield.text = "0";
                }
                else if (inputfield.name == "Time Step")
                {
                    inputfield.text = "0";
                }
                else if (inputfield.name == "Grid Size")
                {
                    inputfield.text = "0";
                }
            }
        }
    }
}
