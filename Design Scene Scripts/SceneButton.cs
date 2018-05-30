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
                else if (inputfield.name == "Width")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().Width.ToString();
                }
                else if (inputfield.name == "Length")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().Length.ToString();
                }
                else if (inputfield.name == "Height")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().Height.ToString();
                }
                else if (inputfield.name == "PlayerX")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().PlayerX.ToString();
                }
                else if (inputfield.name == "PlayerY")
                {
                    inputfield.text = LinkedGameObject.GetComponent<SceneInfo>().PlayerY.ToString();
                }
            }
        }
    }

    public void ResetInfo()
    {
        LinkedGameObject.name = OriginalName;
    }

}
