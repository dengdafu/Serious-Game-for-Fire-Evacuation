using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour {

    public GameObject LinkedGameObject = null;
    public InputField NameField;

    public GameObject RelatedPanel;

    private string OriginalName = null;

    public void OnClick()
    {
        if (LinkedGameObject != null)
        {
            GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
            gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(LinkedGameObject);
            gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(true);
            gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(this.gameObject);
            OriginalName = LinkedGameObject.name;
            NameField.text = LinkedGameObject.name;

            // Load info into the scene detail panel
            InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
            foreach (InputField inputfield in AllInputFields)
            {
                if (inputfield.name == "Name")
                {
                    inputfield.text = LinkedGameObject.name;
                }
                else if (inputfield.name == "Smoke Simulation Time")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().SimulationTime.ToString();
                }
                else if (inputfield.name == "Time step")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().TimeStep.ToString();
                }
                else if (inputfield.name == "Grid size")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().GridSize.ToString();
                }
            }
        }
    }

    public void ResetInfo()
    {
        LinkedGameObject.name = OriginalName;
    }

}
