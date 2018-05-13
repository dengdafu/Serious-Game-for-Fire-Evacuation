using UnityEngine;

public class Ceiling : MonoBehaviour {

    public string Name;
    public float x_pos;
    public float y_pos;
    public float z_pos;
    public float Length;
    public float Width;
    public float Opacity;

    public void FillInfo(string name, float xpos, float ypos, float zpos, float length, float width, float opacity)
    {
        Name = name;
        x_pos = xpos;
        y_pos = ypos;
        z_pos = zpos;
        Length = length;
        Width = width;
        Opacity = opacity;
    }
}
