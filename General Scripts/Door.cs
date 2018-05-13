using UnityEngine;

public class Door : MonoBehaviour {

    public string Name;
    public GameObject WallAttachedTo;
    public GameObject NextScene;
    public bool Open;
    public float RelativePosition;
    public float Width;
    public float Height;

    public void FillInfo(string name, GameObject wall, GameObject scene, bool open, float relpos, float width, float height)
    {
        Name = name;
        WallAttachedTo = wall;
        NextScene = scene;
        Open = open;
        RelativePosition = relpos;
        Width = width;
        Height = height;
    }

}
