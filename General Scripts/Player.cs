﻿using UnityEngine;

public class Player : MonoBehaviour {

    public string Name;
    public float x_pos;
    public float y_pos;
    public float Speed = 1.4f;
    public float Health = 100f;

    public void FillInfo(string name, float xpos, float ypos, float speed, float health)
    {
        Name = name; x_pos = xpos; y_pos = ypos; Speed = speed; Health = health;
    }

}
