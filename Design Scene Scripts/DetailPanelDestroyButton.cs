using UnityEngine;

public class DetailPanelDestroyButton : MonoBehaviour {

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // If the object is "Player", set DesignSceneGameManage.Player back to null
        if (gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder().tag == "Player")
        {
            gamemanager.GetComponent<DesignSceneGameManager>().Player = null;
        }

        // Destroy the currently designed object
        Destroy(gamemanager.GetComponent<DesignSceneGameManager>().GetTempObjectHolder());

        // If it is an existing game object, also destroy the object's associated button

        if (gamemanager.GetComponent<DesignSceneGameManager>().GetIsExistingObejct())
        {
            Destroy(gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton());
        }

        // Reset TempObjectHolder to null, IsExistingObject to false, and LastClickedButton to null
        gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
    }

}
