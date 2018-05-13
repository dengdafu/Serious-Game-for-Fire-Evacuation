using UnityEngine;

public class Wall : MonoBehaviour {

    public string Name;
    public float x_pos;
    public float y_pos;
    public float z_rot;
    public float Height;
    public float Width;
    public float Opacity;

    public void FillInfo(string name, float xpos, float ypos, float zrot, float height, float width, float opacity)
    {
        Name = name;
        x_pos = xpos;
        y_pos = ypos;
        z_rot = zrot;
        Height = height;
        Width = width;
        Opacity = opacity;
    }
}
