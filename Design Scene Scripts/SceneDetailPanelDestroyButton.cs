using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneDetailPanelDestroyButton : MonoBehaviour {

    public GameObject ToolBox;

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        GameObject CurrentObject = gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        GameObject LastClickedButton = gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton();

        // If this is a new scene, then just destroy the scene itself
        if (!gamemanager.GetComponent<DesignSceneGameManager>().GetIsExistingObejct())
        {
            Destroy(CurrentObject);
        }
        // If not, also destroy its associated object buttons and the scene button
        else
        {
            foreach (Transform child in CurrentObject.transform)
            {
                Destroy(child.gameObject.GetComponent<AssociatedButton>().button);
            }
            // Remove it from AllScenes list in game manager
            List<GameObject> TempAllScenes = gamemanager.GetComponent<DesignSceneGameManager>().AllScenes;
            TempAllScenes.Remove(CurrentObject);
            gamemanager.GetComponent<DesignSceneGameManager>().AllScenes = TempAllScenes;
            Destroy(CurrentObject);
            Destroy(LastClickedButton);
        }

        // after destroying the button, disable all the tool buttons except the new scene button

        foreach (Transform child in ToolBox.transform)
        {
            if (child.gameObject.tag == "Button" && child.gameObject.name != "New Scene Button")
            {
                child.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        // Reset TempObjectHolder, IsExistingObject, LastClickedButton in DesignSceneGameManager
        gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);

        // Set Header to be "No scene loaded"
        gamemanager.GetComponent<DesignSceneGameManager>().Header.text = "No scene loaded";
    }
}
