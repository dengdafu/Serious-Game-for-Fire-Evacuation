using UnityEngine;

public class Floor : MonoBehaviour {

    public string Name;
    public float x_pos;
    public float y_pos;
    public float Length;
    public float Width;

    public void FillInfo(string name, float xpos, float ypos, float length, float width)
    {
        Name = name;
        x_pos = xpos;
        y_pos = ypos;
        Length = length;
        Width = width;
    }
}
