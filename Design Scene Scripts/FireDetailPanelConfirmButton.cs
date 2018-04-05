using UnityEngine;
using UnityEngine.UI;

public class FireDetailPanelConfirmButton : MonoBehaviour {

    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public GameObject DetailPanel;
    public GameObject OpenPanel;
    public GameObject ClosePanel;
    public Dropdown Fuel;
    public Button CancelButton;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        InputField[] InputFields = DetailPanel.GetComponentsInChildren<InputField>();
        Dropdown FuelDropdown = DetailPanel.GetComponentInChildren<Dropdown>();
        float HRRPUA = 0;
        float CO_YIELD = 0;
        float SOOT_YIELD = 0;
        float Width = 1;
        float Length = 1;
        int FuelValue = 0;

        foreach (InputField inputfield in InputFields)
        {
            if (inputfield.name == "HRRPUA")
            {
                try
                {
                    HRRPUA = float.Parse(inputfield.text);
                }
                catch { }
            }
            else if (inputfield.name == "CO YIELD")
            {
                try
                {
                    CO_YIELD = float.Parse(inputfield.text);
                }
                catch { }
            }
            else if (inputfield.name == "SOOT YIELD")
            {
                try
                {
                    SOOT_YIELD = float.Parse(inputfield.text);
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
            else if (inputfield.name == "Length")
            {
                try
                {
                    Length = float.Parse(inputfield.text);
                }
                catch { }
            }
        }
        FuelValue = FuelDropdown.value;

        // Set variables in Fire.cs
        CurrentObject.GetComponent<Fire>().HRRPUA = HRRPUA;
        CurrentObject.GetComponent<Fire>().CO_YIELD = CO_YIELD;
        CurrentObject.GetComponent<Fire>().SOOT_YIELD = SOOT_YIELD;
        CurrentObject.GetComponent<Fire>().FUEL = FuelDropdown.options[FuelValue].text;

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
