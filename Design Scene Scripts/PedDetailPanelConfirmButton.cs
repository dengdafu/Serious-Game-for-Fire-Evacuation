using UnityEngine;
using UnityEngine.UI;

public class PedDetailPanelConfirmButton : MonoBehaviour {
    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public GameObject DetailPanel;
    public GameObject OpenPanel;
    public GameObject ClosePanel;
    public GameObject WarningWindow;
    public Button CancelButton;

    public InputField Name;
    public InputField x_pos;
    public InputField y_pos;
    public InputField Speed;
    public InputField Health;
    public InputField Exit;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        string name = Name.text;
        float xpos = float.Parse(x_pos.text);
        float ypos = float.Parse(y_pos.text);
        float speed = float.Parse(Speed.text);
        float health = float.Parse(Health.text);
        string ExitName = Exit.text;


        // if there is no such exit, then the warning window will be popoed up.
        GameObject exit = GameObject.Find(ExitName);
        if (exit == null)
        {
            WarningWindow.SetActive(true);
            WarningWindow.GetComponentInChildren<Text>().text = "No such exit!";
        }
        else
        {
            // Set variables in Pedestrian.cs
            CurrentObject.GetComponent<Pedestrian>().FillInfo(name, xpos, ypos, speed, health, exit);

            // only if the object currently being designed is not an existing object, should a button be
            // added to the object list, otherwise, just use the pre-existed button with the updated name.
            if (!gamemanager.GetComponent<DesignSceneGameManager>().GetIsExistingObejct())
            {
                Button clone = Instantiate(ObjectExampleButton);
                clone.gameObject.SetActive(true);
                clone.transform.SetParent(ObjectList.transform);
                clone.GetComponent<RectTransform>().sizeDelta = ObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                clone.GetComponent<RectTransform>().localEulerAngles = ObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                clone.GetComponent<RectTransform>().localPosition = ObjectExampleButton.GetComponent<RectTransform>().localPosition;
                clone.GetComponent<RectTransform>().localScale = ObjectExampleButton.GetComponent<RectTransform>().localScale;
                clone.GetComponent<ObjectButton>().SetLink(CurrentObject);
                clone.GetComponentInChildren<Text>().text = CurrentObject.name;
                CurrentObject.GetComponent<AssociatedButton>().button = clone.gameObject;
            }
            else
            {
                lastClickedButton.GetComponentInChildren<Text>().text = CurrentObject.name;
            }

            // reset TempObjectHolder and IsExistingObject in DesignSceneGameManager back to default.
            gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
            gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);

            OpenPanel.SetActive(true);
            ClosePanel.SetActive(false);

            if (CancelButton != null)
            {
                CancelButton.interactable = true;
            }

            DetailPanel.GetComponent<DetailPanel>().ResetInputFields();
        }
    }
}
