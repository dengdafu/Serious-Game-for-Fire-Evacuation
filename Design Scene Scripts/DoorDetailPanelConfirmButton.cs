using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorDetailPanelConfirmButton : MonoBehaviour {

    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public GameObject DetailPanel;
    public GameObject OpenPanel;
    public GameObject ClosePanel;
    public GameObject WarningWindow;
    public Toggle OpenToggle;
    public Button CancelButton;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        InputField[] InputFields = DetailPanel.GetComponentsInChildren<InputField>();
        string WallName = "qwertyyyytterwdfdfasdcxcvch"; //just some default name used as placeholder
        string NextScene = "qwertyyyytterwdfdfasdcxcvch";
        float RelativePosition = 0;
        float Width = 1;
        float Height = 1;
        bool Open = OpenToggle.isOn;

        foreach (InputField inputfield in InputFields)
        {
            if (inputfield.name == "Wall")
            {
                WallName = inputfield.text;
            }
            else if (inputfield.name == "Next Scene")
            {
                NextScene = inputfield.text;
            }
            else if (inputfield.name == "Relative Position")
            {
                try
                {
                    RelativePosition = float.Parse(inputfield.text);
                }
                catch { }
            }
            else if (inputfield.name == "Width")
            {
                try
                {
                    Width = float.Parse(inputfield.text);
                }
                catch { }
            }
            else if (inputfield.name == "Height")
            {
                try
                {
                    Height = float.Parse(inputfield.text);
                }
                catch { }
            }
        }

        // if there is no such wall or no such next scene, then the warning window will be popoed up.
        GameObject wall = GameObject.Find(WallName);
        GameObject scene = null;
        foreach (GameObject gameobject in gamemanager.GetComponent<DesignSceneGameManager>().AllScenes)
        {
            if (gameobject != null && gameobject.name == NextScene)
            {
                scene = gameobject;
            }
        }
        if (wall == null)
        {
            WarningWindow.SetActive(true);
            WarningWindow.GetComponentInChildren<Text>().text = "No such wall!";
        }
        else if (scene == null)
        {
            WarningWindow.SetActive(true);
            WarningWindow.GetComponentInChildren<Text>().text = "No such scene!";
        }
        else
        {
            // Set variables in Door.cs
            CurrentObject.GetComponent<Door>().WallAttachedTo = wall;
            CurrentObject.GetComponent<Door>().NextScene = scene;
            CurrentObject.GetComponent<Door>().Open = Open;

            // Set up the door object correctly
            CurrentObject.transform.SetParent(wall.transform);
            Vector3 WallPosition = wall.transform.position;
            Vector3 WallDimensions = wall.transform.localScale;
            CurrentObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            float rel_x = RelativePosition / WallDimensions.x;
            float rel_y = ((Height / 2) - WallPosition.y) / WallDimensions.y;
            CurrentObject.transform.localPosition = new Vector3(rel_x,rel_y,0);
            CurrentObject.transform.localScale = new Vector3(Width/WallDimensions.x,Height/WallDimensions.y,1.033f);

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
