﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {

    public GameObject gamemanager;

    public void Save()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "", FileMode.Open);
    }

}

[Serializable]
class wall
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float zrot;
    public float Width;
    public float Height;
    public float Opacity;
}

[Serializable]
class floor
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float Width;
    public float Length;
}

[Serializable]
class ceiling
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float zpos;
    public float Opacity;
    public int Open;
    public float Width;
    public float Length;
}

[Serializable]
class obstacle
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float Width;
    public float Length;
    public float Height;
    public float Opacity;
}

[Serializable]
class door
{
    public int NameIndex;
    public int WallNameIndex;
    public int SceneNameIndex;
    public int Open;
    public float Width;
    public float Height;
}

[Serializable]
class fire
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float zpos;
    public float Width;
    public float Length;
    public float HRRPUA;
    public int Fuel;
    public float CO_YIELD;
    public float SOOT_YIELD;
}

[Serializable]
class pedestrian
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public int ExitNameIndex;
    public float Speed;
    public float Health;
}

