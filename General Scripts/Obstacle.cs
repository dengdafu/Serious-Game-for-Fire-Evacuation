using UnityEngine;

public class Obstacle : MonoBehaviour {

    public string Name;
    public float x_pos;
    public float y_pos;
    public float Width;
    public float Length;
    public float Height;
    public float Opacity;

    public void FillInfo(string name, float xpos, float ypos, float width, float length, float height, float opacity)
    {
        Name = name;
        x_pos = xpos;
        y_pos = ypos;
        Width = width;
        Length = length;
        Height = height;
        Opacity = opacity;
    }
}
