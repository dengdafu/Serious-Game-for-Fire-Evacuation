using UnityEngine;

public class ToolBoxFireButton : MonoBehaviour {

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initiate the Fire object and put it under the current scene
        GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/Fire"));
        Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated fire
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
