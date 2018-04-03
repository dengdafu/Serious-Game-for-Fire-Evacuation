using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneGameManager : MonoBehaviour {
    public void PlaySceneChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
