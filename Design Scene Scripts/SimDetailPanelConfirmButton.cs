using UnityEngine;
using UnityEngine.UI;

public class SimDetailPanelConfirmButton : MonoBehaviour {

    public GameObject GameManager;
    public Dropdown dropdown;
    public InputField SimulationTime;
    public InputField TimeStep;
    public InputField GridSize;

    public void OnClick()
    {
        try
        {
            float SimTime = float.Parse(SimulationTime.text);
            float timeStep = float.Parse(TimeStep.text);
            float gridSize = float.Parse(GridSize.text);

            int CurrentIndex = dropdown.GetComponent<SceneDropdown>().CurrentIndex;
            GameManager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo[CurrentIndex] =
                new DesignSceneGameManager.SceneSimInfo(true, SimTime, timeStep, gridSize);
        }
        catch { }
    }
}
