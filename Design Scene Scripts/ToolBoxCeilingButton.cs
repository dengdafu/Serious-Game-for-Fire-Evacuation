using UnityEngine;

public class ToolBoxCeilingButton : MonoBehaviour {

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initiate the ceiling object and put it under the currrent scene
        GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/Ceiling"));
        Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated wall
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
