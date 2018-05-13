using UnityEngine;
using UnityEngine.UI;

public class DoorDetailPanelConfirmButton : MonoBehaviour {

    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public GameObject DetailPanel;
    public GameObject OpenPanel;
    public GameObject ClosePanel;
    public GameObject WarningWindow;
    public Button CancelButton;

    public InputField Name;
    public InputField Wall;
    public InputField Scene;
    public InputField RelativePosition;
    public Toggle OpenToggle;
    public InputField Width;
    public InputField Height;


    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        string WallName = Wall.text;
        string NextScene = Scene.text;
        float RelPos = float.Parse(RelativePosition.text);
        float width = float.Parse(Width.text);
        float height = float.Parse(Height.text);
        bool Open = OpenToggle.isOn;

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
            CurrentObject.GetComponent<Door>().FillInfo(Name.text, wall, scene, Open, RelPos, width, height);

            // Set up the door object correctly
            CurrentObject.transform.SetParent(wall.transform);
            Vector3 WallPosition = wall.transform.position;
            Vector3 WallDimensions = wall.transform.localScale;
            CurrentObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            float rel_x = RelPos / WallDimensions.x;
            float rel_y = ((height / 2) - WallPosition.y) / WallDimensions.y;
            CurrentObject.transform.localPosition = new Vector3(rel_x,rel_y,0);
            CurrentObject.transform.localScale = new Vector3(width/WallDimensions.x,height/WallDimensions.y,1.033f);

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
