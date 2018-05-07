using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {

    public GameObject gamemanager;
    public InputField ScenarioName;
    private string WorkingDirectory;

    public void SaveOnClick()
    {
        // 1. Get the SaveName;
        string scenarioName = ScenarioName.text;

        // 2. Get all the existing scenes from gamemanager.
        List<GameObject> AllScenes = null;
        if (gamemanager.name == "DesignSceneGameManager")
        {
            AllScenes = gamemanager.GetComponent<DesignSceneGameManager>().AllScenes;
        }

        // 3. Create a folder named with savename.
        WorkingDirectory = Application.persistentDataPath + "/" + scenarioName;
        Directory.CreateDirectory(WorkingDirectory);

        // 4. For each scene, do the followings
        for (int i = 0; i < AllScenes.Count; i++)
        {
            // 4.1 create a sub-folder with the scene name
            WorkingDirectory = Application.persistentDataPath + scenarioName + AllScenes[i].name;
            Directory.CreateDirectory(WorkingDirectory);

            // 4.2 create a list to store all the names of the objects
            List<string> AllObjectNames = new List<string>();

            // 4.3 Fill in the class SceneDetails according to the design of the scene
            SceneDetails sceneDetails = new SceneDetails();
            // 4.3.1 Fill in the simulation info
            sceneDetails.SimTime = gamemanager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo[i].SimulationTime;
            sceneDetails.TimeStep = gamemanager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo[i].TimeStep;
            sceneDetails.GridSize = gamemanager.GetComponent<DesignSceneGameManager>().AllSceneSimInfo[i].GridSize;
            // 4.3.2 fill in the info of different objects into sceneDetails
            foreach (Transform ChildObjectTransform in AllScenes[i].transform)
            {
                // Wall and its doors
                if (ChildObjectTransform.tag == "Wall")
                {
                    wall WallInfo = new wall();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    WallInfo.NameIndex = AllObjectNames.Count;
                    WallInfo.xpos = ChildObjectTransform.position.x;
                    WallInfo.ypos = ChildObjectTransform.position.z;
                    WallInfo.zrot = ChildObjectTransform.rotation.y;
                    WallInfo.Width = ChildObjectTransform.localScale.x;
                    WallInfo.Height = ChildObjectTransform.localScale.y;
                    WallInfo.Opacity = ChildObjectTransform.GetComponent<Renderer>().material.color.a;
                    sceneDetails.Walls.Add(WallInfo);

                    // doors that are attached to it
                    foreach (Transform doorTransfrom in ChildObjectTransform)
                    {
                        if (doorTransfrom.tag == "Door")
                        {
                            door DoorInfo = new door();
                            AllObjectNames.Add(doorTransfrom.gameObject.name);
                            DoorInfo.NameIndex = AllObjectNames.Count;
                            DoorInfo.RelativePosition = doorTransfrom.localPosition.x;
                            DoorInfo.Width = doorTransfrom.localScale.x;
                            DoorInfo.Height = doorTransfrom.localScale.y;
                            if (doorTransfrom.gameObject.GetComponent<Door>().Open)
                            {
                                DoorInfo.Open = 1;
                            }
                            else
                            {
                                DoorInfo.Open = 0;
                            }
                            DoorInfo.WallNameIndex = WallInfo.NameIndex;
                            AllObjectNames.Add(doorTransfrom.gameObject.GetComponent<Door>().NextScene.name);
                            DoorInfo.SceneNameIndex = AllObjectNames.Count;
                            sceneDetails.Doors.Add(DoorInfo);
                        }
                    }
                }
                // Floor
                else if (ChildObjectTransform.tag == "Floor")
                {
                    floor FloorInfo = new floor();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    FloorInfo.NameIndex = AllObjectNames.Count;
                    FloorInfo.xpos = ChildObjectTransform.position.x;
                    FloorInfo.ypos = ChildObjectTransform.position.z;
                    FloorInfo.Width = ChildObjectTransform.localScale.x;
                    FloorInfo.Length = ChildObjectTransform.localScale.z;
                    sceneDetails.Floors.Add(FloorInfo);
                }
                // Ceiling
                else if (ChildObjectTransform.tag == "Ceiling")
                {
                    ceiling CeilingInfo = new ceiling();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    CeilingInfo.NameIndex = AllObjectNames.Count;
                    CeilingInfo.xpos = ChildObjectTransform.position.x;
                    CeilingInfo.ypos = ChildObjectTransform.position.z;
                    CeilingInfo.zpos = ChildObjectTransform.position.y;
                    CeilingInfo.Width = ChildObjectTransform.localScale.x;
                    CeilingInfo.Length = ChildObjectTransform.localScale.z;
                    // since the functionality of "open" is yet to be properly assigned, "open" = 0 for now.
                    CeilingInfo.Open = 0;
                    CeilingInfo.Opacity = ChildObjectTransform.GetComponent<Renderer>().material.color.a;
                    sceneDetails.Ceilings.Add(CeilingInfo);
                }
                // Obstacle
                else if (ChildObjectTransform.tag == "Obstacle")
                {
                    obstacle ObstacleInfo = new obstacle();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    ObstacleInfo.NameIndex = AllObjectNames.Count;
                    ObstacleInfo.xpos = ChildObjectTransform.position.x;
                    ObstacleInfo.ypos = ChildObjectTransform.position.z;
                    ObstacleInfo.Width = ChildObjectTransform.localScale.x;
                    ObstacleInfo.Length = ChildObjectTransform.localScale.z;
                    ObstacleInfo.Height = ChildObjectTransform.localScale.y;
                    ObstacleInfo.Opacity = ChildObjectTransform.GetComponent<Renderer>().material.color.a;
                    sceneDetails.Obstacles.Add(ObstacleInfo);
                }
                // Fire
                else if (ChildObjectTransform.tag == "Fire")
                {
                    fire FireInfo = new fire();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    FireInfo.NameIndex = AllObjectNames.Count;
                    FireInfo.xpos = ChildObjectTransform.position.x;
                    FireInfo.ypos = ChildObjectTransform.position.z;
                    FireInfo.zpos = ChildObjectTransform.position.y;
                    FireInfo.Width = ChildObjectTransform.localScale.x;
                    FireInfo.Length = ChildObjectTransform.localScale.z;
                    FireInfo.HRRPUA = ChildObjectTransform.gameObject.GetComponent<Fire>().HRRPUA;
                    FireInfo.CO_YIELD = ChildObjectTransform.gameObject.GetComponent<Fire>().CO_YIELD;
                    FireInfo.SOOT_YIELD = ChildObjectTransform.gameObject.GetComponent<Fire>().SOOT_YIELD;
                    FireInfo.Fuel = Array.IndexOf(ChildObjectTransform.gameObject.GetComponent<Fire>().Fuels,
                        ChildObjectTransform.gameObject.GetComponent<Fire>().FUEL);
                    sceneDetails.Fires.Add(FireInfo);
                }
                // Pedestrian
                else if (ChildObjectTransform.tag == "Pedestrian")
                {
                    pedestrian PedestrianInfo = new pedestrian();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    PedestrianInfo.NameIndex = AllObjectNames.Count;
                    PedestrianInfo.xpos = ChildObjectTransform.position.x;
                    PedestrianInfo.ypos = ChildObjectTransform.position.z;
                    PedestrianInfo.Speed = ChildObjectTransform.gameObject.GetComponent<Pedestrian>().Speed;
                    PedestrianInfo.Health = ChildObjectTransform.gameObject.GetComponent<Pedestrian>().Health;
                    AllObjectNames.Add(ChildObjectTransform.gameObject.GetComponent<Pedestrian>().Exit.name);
                    PedestrianInfo.ExitNameIndex = AllObjectNames.Count;
                    sceneDetails.Pedestrians.Add(PedestrianInfo);
                }
            }

            // 4.4 Save AllObjectNames into a text file
            StreamWriter NamesTxtFile = new StreamWriter(WorkingDirectory + "/AllNames.txt");
            foreach (string name in AllObjectNames)
            {
                NamesTxtFile.WriteLine(name);
            }
            NamesTxtFile.Close();

            // 4.5 Save sceneDetails into a binary file, SceneDetails.dat
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(WorkingDirectory + "/SceneDetails.dat", FileMode.Open);
            bf.Serialize(file, sceneDetails);
            file.Close();
        }
    }

}

[Serializable]
class SceneDetails
{
    public float SimTime;
    public float TimeStep;
    public float GridSize;
    public List<wall> Walls;
    public List<floor> Floors;
    public List<ceiling> Ceilings;
    public List<obstacle> Obstacles;
    public List<door> Doors;
    public List<fire> Fires;
    public List<pedestrian> Pedestrians;
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
    public float RelativePosition;
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