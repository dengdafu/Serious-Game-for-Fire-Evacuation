using UnityEngine;
using UnityEngine.UI;

public class ToolBoxPlayerButton : MonoBehaviour {

    public GameObject OpenPanel;
    public GameObject ClosePanel;
    public Button CancelButton;
    public GameObject WarningWindow;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        if (gamemanager.GetComponent<DesignSceneGameManager>().Player != null)
        {
            WarningWindow.SetActive(true);
            WarningWindow.GetComponent<Text>().text = "A player has already existed!";
        }
        else
        {
            // Open OpenPanel, close ClosePanel, disable CancelButton
            OpenPanel.SetActive(true);
            ClosePanel.SetActive(false);
            CancelButton.interactable = false;

            // Initiate the player object and put it under the current scene
            GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/DesignScenePlayer"));
            Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

            // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated player
            gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

            // Set DesignSceneGameMangager.Player to be this player
            gamemanager.GetComponent<DesignSceneGameManager>().Player = Object;

            // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
            gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        }
    }
}
