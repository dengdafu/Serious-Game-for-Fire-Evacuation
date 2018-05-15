using UnityEngine;

public class ToolBoxPedestrianButton : MonoBehaviour {

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initiate the pedestrian object and put it under the current scene
        GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/DesignScenePedestrian"));
        Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated pedestrian
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
