using UnityEngine;

public class SceneDetailPanelCancelButton : MonoBehaviour {

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton().GetComponent<SceneButton>().ResetInfo();
        gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
    }

}
