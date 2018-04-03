using UnityEngine;
using UnityEngine.UI;

public class SetButtonInteratable : MonoBehaviour {

    public void setButtonInteractable(Button button)
    {
        button.interactable = true;
    }

    public void setButtonNotInteractable(Button button)
    {
        button.interactable = false;
    }
}
