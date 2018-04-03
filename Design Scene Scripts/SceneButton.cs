using UnityEngine;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour {

    public GameObject LinkedGameObject = null;
    public InputField NameField;

    private string OriginalName = null;

    public void OnClick()
    {
        if (LinkedGameObject != null)
        {
            GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
            OriginalName = LinkedGameObject.name;
            NameField.text = LinkedGameObject.name;
            gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(LinkedGameObject);
            gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(true);
            gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(this.gameObject);
        }
    }

    public void ResetInfo()
    {
        LinkedGameObject.name = OriginalName;
    }

}
