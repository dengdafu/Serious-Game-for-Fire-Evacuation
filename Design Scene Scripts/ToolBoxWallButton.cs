using UnityEngine;

public class ToolBoxWallButton : MonoBehaviour {

    public void Onclick()
    {     
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initiate the wall object and put it under the currrent scene
        GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/Wall"));
        Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated wall
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
