using UnityEngine;

public class ActivateGameObject : MonoBehaviour {

    public void Activate(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void Deactivate(GameObject gameobject)
    {
        gameobject.SetActive(false);
    }
}
