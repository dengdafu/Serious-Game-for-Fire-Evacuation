using UnityEngine;
using UnityEngine.UI;

public class FireDetailPanelConfirmButton : MonoBehaviour {

    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public GameObject DetailPanel;
    public GameObject OpenPanel;
    public GameObject ClosePanel;
    // public Dropdown Fuel;
    public Button CancelButton;

    public InputField Name;
    public Dropdown Fuel;
    public InputField xpos;
    public InputField ypos;
    public InputField zpos;
    public InputField Width;
    public InputField Length;
    public InputField HRRPUA;
    public InputField SOOT_YIELD;
    public InputField CO_YIELD;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        string name = Name.text;
        int FuelValue = Fuel.value;
        string fuel = Fuel.options[FuelValue].text;
        float x_pos = float.Parse(xpos.text);
        float y_pos = float.Parse(ypos.text);
        float z_pos = float.Parse(zpos.text);
        float hrrpua = float.Parse(HRRPUA.text);
        float width = float.Parse(Width.text);
        float length = float.Parse(Length.text);
        float soot_yield = float.Parse(SOOT_YIELD.text);
        float co_yield = float.Parse(CO_YIELD.text);

        // Set variables in Fire.cs
        CurrentObject.GetComponent<Fire>().FillInfo(name, x_pos, y_pos, z_pos, width, length,
            hrrpua, co_yield, soot_yield, fuel);

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
