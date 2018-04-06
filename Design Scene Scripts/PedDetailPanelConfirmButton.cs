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

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        InputField[] InputFields = DetailPanel.GetComponentsInChildren<InputField>();
        string ExitName = "qwertyyyytterwdfdfasdcxcvch"; //just some default name used as placeholder
        float Speed = 1.4f;
        float Health = 100f;

        foreach (InputField inputfield in InputFields)
        {
            if (inputfield.name == "Exit")
            {
                ExitName = inputfield.text;
            }
            else if (inputfield.name == "Speed")
            {
                try
                {
                    Speed = float.Parse(inputfield.text);
                }
                catch { }
            }
            else if (inputfield.name == "Health")
            {
                try
                {
                    Health = float.Parse(inputfield.text);
                }
                catch { }
            }
        }

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
            CurrentObject.GetComponent<Pedestrian>().Exit = exit;
            CurrentObject.GetComponent<Pedestrian>().Health = Health;
            CurrentObject.GetComponent<Pedestrian>().Speed = Speed;

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
