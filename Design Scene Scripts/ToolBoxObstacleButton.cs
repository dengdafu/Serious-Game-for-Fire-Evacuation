﻿using UnityEngine;

public class ToolBoxObstacleButton : MonoBehaviour {

    public void Onclick()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        // Initiate the obstacle object and put it under the currrent scene
        GameObject Object = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"));
        Object.transform.SetParent(gamemanager.GetComponent<DesignSceneGameManager>().GetCurrentScene().transform);

        // Set "TempObjectHolder" in DesignSceneGameManager to be this initiated wall
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(Object);

        // Set "IsExistingObject" to be false, indicating that we are not looking at a pre-created game object
        gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(false);
    }
}
