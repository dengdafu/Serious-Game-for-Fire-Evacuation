using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneGameManager : MonoBehaviour {

    public void LoadSceneChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
