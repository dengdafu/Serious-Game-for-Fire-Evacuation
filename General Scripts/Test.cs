using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

    public string SceneName;

	// Use this for initialization
	void Start () {
        Debug.Log(SceneName);		
	}

    public void OnClick()
    {
        Debug.Log("HI");
        SceneManager.LoadScene(SceneName);
    }
}
