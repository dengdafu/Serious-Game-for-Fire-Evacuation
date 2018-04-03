using UnityEngine;

public class DetailPanelCancelButton : MonoBehaviour {

    public void OnClick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        gamemanager.GetComponent<DesignSceneGameManager>().GetLastClickedButton().GetComponent<ObjectButton>().ResetInfo();
        gamemanager.GetComponent<DesignSceneGameManager>().ResetTempObjectHolder();
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
    }
        

}
