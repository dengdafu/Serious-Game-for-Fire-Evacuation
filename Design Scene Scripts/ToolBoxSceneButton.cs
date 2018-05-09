using UnityEngine;

public class ToolBoxSceneButton : MonoBehaviour {

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Create an empty game object, called "Unnamed scene"
        GameObject scene = new GameObject("Unnamed scene");
        scene.AddComponent<AssociatedButton>();
        scene.AddComponent<SceneInfo>();
        scene.tag = "Scene";

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated wall
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(scene);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
