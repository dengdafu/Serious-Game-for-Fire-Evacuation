using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public void changescene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
