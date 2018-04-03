using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DetailPanelConfirmButton : MonoBehaviour {

    public GameObject ObjectList;
    public Button ObjectExampleButton;

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject lastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

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
    }
}
