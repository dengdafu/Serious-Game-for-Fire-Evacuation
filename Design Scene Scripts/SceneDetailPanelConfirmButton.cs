using UnityEngine;
using UnityEngine.UI;

public class SceneDetailPanelConfirmButton : MonoBehaviour {

    public GameObject SceneList;
    public GameObject SceneButtonInstance;

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();

        // Inactivate the current scene and its associated object buttons, then set current scene to this scene.
        GameObject CurrentScene = gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene();
        GameObject LastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();
        if (CurrentScene != null)
        {
            foreach (Transform child in CurrentScene.transform)
            {
                child.gameObject.GetComponent<AssociatedButton>().button.SetActive(false);
            }
            CurrentScene.SetActive(false);
        }
        gamemanager.GetComponent<DesignSceneGameManager>().SetCurrentScene(CurrentObject);
        // If it is not about an existing scene, create a scene button in the scene list in the design panel, which links to this scene
        // Also, add this scene into the AllScenes list in GameManager.
        if (!gamemanager.GetComponent<DesignSceneGameManager>().GetIsExistingObejct())
        {
            GameObject NewSceneButton = Instantiate(SceneButtonInstance);
            NewSceneButton.SetActive(true);
            NewSceneButton.transform.SetParent(SceneList.transform);
            NewSceneButton.GetComponentInChildren<Text>().text = CurrentObject.name;
            NewSceneButton.transform.position = SceneButtonInstance.transform.position;
            NewSceneButton.transform.localEulerAngles = SceneButtonInstance.transform.localEulerAngles;
            NewSceneButton.transform.localScale = SceneButtonInstance.transform.localScale;
            NewSceneButton.GetComponent<SceneButton>().LinkedGameObject = CurrentObject;
            CurrentObject.GetComponent<AssociatedButton>().button = NewSceneButton;

            gamemanager.GetComponent<DesignSceneGameManager>().AllScenes.Add(CurrentObject);
        }
        else
        {
            CurrentObject.SetActive(true);
            CurrentObject.GetComponent<AssociatedButton>().button.GetComponentInChildren<Text>().text = CurrentObject.name;
            foreach (Transform child in CurrentObject.transform)
            {
                child.gameObject.GetComponent<AssociatedButton>().button.SetActive(true);
            }

        }
        // Update the Header name to this scene's name
        gamemanager.GetComponent<DesignSceneGameManager>().Header.GetComponentInChildren<Text>().text = CurrentObject.name;
        // Reset TempObjectHolder, IsExistingObject, LastClickedButton in DesignSceneGameManager
        gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
        // Enable all buttons in the tool box, because these buttons might have been disabled after destroying a scene.
        GameObject ToolBox = GameObject.Find("Tool Box Sub Panel");
        foreach (Transform tool in ToolBox.transform)
        {
            if (tool.gameObject.tag == "Button")
            {
                tool.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }
}