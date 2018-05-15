using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class InPlaySceneGameManager : MonoBehaviour {

    public bool ScenarioLoaded = false;

    /*private void Start()
    {
        // Add Scene
        GameObject scene = new GameObject("Scene");
        scene.tag = "Scene";
        scene.transform.position = new Vector3(0, 0, 0);

        GameObject newFloor = Instantiate(Resources.Load<GameObject>("Prefabs/InPlayFloor"));
        newFloor.name = "Test floor";
        newFloor.transform.SetParent(scene.transform);

        Vector3 Scale = newFloor.GetComponent<Terrain>().terrainData.size;
        Scale.x = 10; Scale.z = 10;
        newFloor.GetComponent<Terrain>().terrainData.size = Scale;

        Vector3 Pos = newFloor.transform.position;
        Pos.x = 0 - (Scale.x / 2); Pos.z = 0 - (Scale.z / 2);
        newFloor.transform.position = Pos;

        // Bake the floor
        newFloor.GetComponent<NavMeshLink>().startPoint = new Vector3(0, 0, 0);
        newFloor.GetComponent<NavMeshLink>().endPoint = new Vector3(10, 0, 10);
        newFloor.GetComponent<NavMeshSurface>().BuildNavMesh();

        // Obstacle
        GameObject newObstacle = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"));
        newObstacle.name = "Obstacle";
        newObstacle.transform.SetParent(scene.transform);

        Vector3 ObPos = newObstacle.transform.position;
        ObPos.x = 5; ObPos.z = 5;
        newObstacle.transform.position = ObPos;

        Vector3 ObScale = newObstacle.transform.localScale;
        Scale.x = 1; Scale.y = 1; Scale.z = 1;
        newObstacle.transform.localScale = Scale;

        // Agent
        GameObject newPedestrian = Instantiate(Resources.Load<GameObject>("Prefabs/Pedestrian"));

        newPedestrian.name = "Susan";
        newPedestrian.transform.SetParent(scene.transform);

        Vector3 PedPos = newPedestrian.transform.position;
        PedPos.x = -5; PedPos.z = -5;
        newPedestrian.transform.position = Pos;

        newPedestrian.GetComponent<AICharacterControl>().target = newObstacle.transform;
    }*/

    private void Start()
    {
        // 1. Set WorkingDirectory
        string ScenarioName = ScenarioAtHand.ScenarioName;
        string WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName;

        // 2. Load in all scenes in the scenario
        DirectoryInfo dir = new DirectoryInfo(WorkingDirectory);
        DirectoryInfo[] subdirs = dir.GetDirectories();
        List<string> AllScenes = new List<string>();
        foreach (DirectoryInfo subdir in subdirs)
        {
            AllScenes.Add(subdir.Name);
        }

        // 3. For each scene, create a scene object and put it in AllSceneObjects
        List<GameObject> AllSceneObjects = new List<GameObject>();
        foreach (string Scene in AllScenes)
        {
            GameObject scene = new GameObject(Scene);
            scene.tag = "Scene";
            scene.transform.position = new Vector3(0, 0, 0);
            scene.AddComponent<SceneInfo>();
            AllSceneObjects.Add(scene);
        }

        // 4. For each scene, load the stored data
        foreach (GameObject Scene in AllSceneObjects)
        {
            WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName + "/" + Scene.name;
            List<string> AllObjectsNames = new List<string>();

            // 4.1 read all the names from the AllNames.txt file.
            if (File.Exists(WorkingDirectory + "/AllNames.txt"))
            {
                string name;
                StreamReader theReader = new StreamReader(WorkingDirectory + "/AllNames.txt");
                using (theReader)
                {
                    do
                    {
                        name = theReader.ReadLine();
                        if (name != null)
                        {
                            AllObjectsNames.Add(name);
                        }
                    }
                    while (name != null);
                }
                theReader.Close();
            }

            // 4.2 read in all the details into sceneDetails (SceneDetails)
            SceneDetails sceneDetails = new SceneDetails();
            if (File.Exists(WorkingDirectory + "/SceneDetails.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(WorkingDirectory + "/SceneDetails.dat", FileMode.Open);
                sceneDetails = (SceneDetails)bf.Deserialize(file);
                file.Close();
            }

            // 4.3 Recover the scene based on sceneDetails
            // 4.3.1 Fill in sim info. If it is in design scene, then need AddComponent<AssociatedButton>() as well.
            Scene.GetComponent<SceneInfo>().SimulationTime = sceneDetails.SimTime;
            Scene.GetComponent<SceneInfo>().TimeStep = sceneDetails.TimeStep;
            Scene.GetComponent<SceneInfo>().GridSize = sceneDetails.GridSize;

            // 4.3.2 Create all the walls
            foreach (wall Wall in sceneDetails.Walls)
            {
                GameObject newWall = Instantiate(Resources.Load<GameObject>("Prefabs/Wall"));
                Wall wallinfo = newWall.GetComponent<Wall>();
                wallinfo.FillInfo(AllObjectsNames[Wall.NameIndex], Wall.xpos, Wall.ypos,
                    Wall.zrot, Wall.Height, Wall.Width, Wall.Opacity);
                newWall.name = wallinfo.Name;
                newWall.transform.SetParent(Scene.transform);

                Vector3 Pos = newWall.transform.position;
                Pos.x = wallinfo.x_pos; Pos.y = wallinfo.Height / 2; Pos.z = wallinfo.y_pos;
                newWall.transform.position = Pos;

                Vector3 Angles = newWall.transform.eulerAngles;
                Angles.y = wallinfo.z_rot;
                newWall.transform.eulerAngles = Angles;

                Vector3 Scale = newWall.transform.localScale;
                Scale.x = wallinfo.Width; Scale.y = wallinfo.Height;
                newWall.transform.localScale = Scale;

                Vector4 color = newWall.GetComponent<Renderer>().material.color;
                color[3] = wallinfo.Opacity;
                newWall.GetComponent<Renderer>().material.color = color;
            }

            // 4.3.3 Create all the floors
            foreach (floor Floor in sceneDetails.Floors)
            {
                GameObject newFloor = Instantiate(Resources.Load<GameObject>("Prefabs/InPlayFloor"));
                Floor floorinfo = newFloor.GetComponent<Floor>();
                floorinfo.FillInfo(AllObjectsNames[Floor.NameIndex], Floor.xpos, Floor.ypos, Floor.Length,
                    Floor.Width);
                newFloor.name = floorinfo.Name;
                newFloor.transform.SetParent(Scene.transform);

                Vector3 Scale = newFloor.GetComponent<Terrain>().terrainData.size;
                Scale.x = floorinfo.Width; Scale.z = floorinfo.Length;
                newFloor.GetComponent<Terrain>().terrainData.size = Scale;

                Vector3 Pos = newFloor.transform.position;
                Pos.x = floorinfo.x_pos - (Scale.x / 2); Pos.z = floorinfo.y_pos - (Scale.z / 2);
                newFloor.transform.position = Pos;

                // Bake the floor
                newFloor.GetComponent<NavMeshLink>().startPoint = new Vector3(0, 0, 0);
                newFloor.GetComponent<NavMeshLink>().endPoint = new Vector3(Scale.x, 0, Scale.z);
                newFloor.GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            // 4.3.4 Create all the ceilings
            foreach (ceiling Ceiling in sceneDetails.Ceilings)
            {
                GameObject newCeiling = Instantiate(Resources.Load<GameObject>("Prefabs/Ceiling"));
                Ceiling ceilinginfo = newCeiling.GetComponent<Ceiling>();
                ceilinginfo.FillInfo(AllObjectsNames[Ceiling.NameIndex], Ceiling.xpos, Ceiling.ypos,
                    Ceiling.zpos, Ceiling.Length, Ceiling.Width, Ceiling.Opacity);
                newCeiling.name = ceilinginfo.Name;
                newCeiling.transform.SetParent(Scene.transform);

                Vector3 Pos = newCeiling.transform.position;
                Pos.x = ceilinginfo.x_pos; Pos.y = ceilinginfo.z_pos; Pos.z = ceilinginfo.y_pos;
                newCeiling.transform.position = Pos;

                Vector3 Scale = newCeiling.transform.localScale;
                Scale.x = ceilinginfo.Width; Scale.z = ceilinginfo.Length;
                newCeiling.transform.localScale = Scale;
            }

            // 4.3.5 Create all the obstacles
            foreach (obstacle Obstacle in sceneDetails.Obstacles)
            {
                GameObject newObstacle = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"));
                Obstacle obstacleinfo = newObstacle.GetComponent<Obstacle>();
                obstacleinfo.FillInfo(AllObjectsNames[Obstacle.NameIndex], Obstacle.xpos, Obstacle.ypos,
                    Obstacle.Width, Obstacle.Length, Obstacle.Height, Obstacle.Opacity);
                newObstacle.name = obstacleinfo.Name;
                newObstacle.transform.SetParent(Scene.transform);

                Vector3 Pos = newObstacle.transform.position;
                Pos.x = obstacleinfo.x_pos; Pos.z = obstacleinfo.y_pos;
                newObstacle.transform.position = Pos;

                Vector3 Scale = newObstacle.transform.localScale;
                Scale.x = obstacleinfo.Width; Scale.y = obstacleinfo.Height; Scale.z = obstacleinfo.Length;
                newObstacle.transform.localScale = Scale;

                Vector4 color = newObstacle.GetComponent<Renderer>().material.color;
                color[3] = obstacleinfo.Opacity;
                newObstacle.GetComponent<Renderer>().material.color = color;
            }

            // 4.3.6 Create all the doors
            foreach (door Door in sceneDetails.Doors)
            {
                GameObject newDoor = Instantiate(Resources.Load<GameObject>("Prefabs/Door"));
                Door doorinfo = newDoor.GetComponent<Door>();

                // Fill in the values of variables in Door.cs
                doorinfo.Name = AllObjectsNames[Door.NameIndex];
                doorinfo.WallAttachedTo = Scene.transform.Find(AllObjectsNames[Door.WallNameIndex]).gameObject;
                foreach (GameObject sceneobj in AllSceneObjects)
                {
                    if (sceneobj.name == AllObjectsNames[Door.SceneNameIndex])
                    {
                        doorinfo.NextScene = sceneobj;
                        break;
                    }
                }
                if (Door.Open == 1)
                {
                    doorinfo.Open = true;
                }
                else
                {
                    doorinfo.Open = false;
                }
                doorinfo.Width = Door.Width;
                doorinfo.Height = Door.Height;
                doorinfo.RelativePosition = Door.RelativePosition;

                // Create the door gameobject accordingly
                newDoor.name = doorinfo.Name;

                Transform WallTransform = doorinfo.WallAttachedTo.transform;
                newDoor.transform.SetParent(WallTransform);
                Vector3 WallPosition = WallTransform.position;
                Vector3 WallDimensions = WallTransform.localScale;

                newDoor.transform.localEulerAngles = new Vector3(0, 0, 0); // angle

                float rel_x = doorinfo.RelativePosition / WallDimensions.x;
                float rel_y = ((doorinfo.Height / 2) - WallPosition.y) / WallDimensions.y;
                newDoor.transform.localPosition = new Vector3(rel_x, rel_y, 0); // pos

                newDoor.transform.localScale = new Vector3(doorinfo.Width / WallDimensions.x,
                     doorinfo.Height / WallDimensions.y, 1.033f); //scale
            }

            // 4.3.7 Create all the fires
            foreach (fire Fire in sceneDetails.Fires)
            {
                GameObject newFire = Instantiate(Resources.Load<GameObject>("Prefabs/Fire"));
                Fire fireinfo = newFire.GetComponent<Fire>();

                fireinfo.FillInfo(AllObjectsNames[Fire.NameIndex], Fire.xpos, Fire.ypos, Fire.zpos, Fire.Width,
                    Fire.Length, Fire.HRRPUA, Fire.CO_YIELD, Fire.SOOT_YIELD, fireinfo.Fuels[Fire.Fuel]);
                newFire.name = fireinfo.Name;
                newFire.transform.SetParent(Scene.transform);

                Vector3 Pos = newFire.transform.position;
                Pos.x = fireinfo.x_pos; Pos.y = fireinfo.z_pos; Pos.z = fireinfo.y_pos;
                newFire.transform.position = Pos;

                Vector3 Scale = newFire.transform.localScale;
                Scale.x = fireinfo.Width; Scale.z = fireinfo.Length;
                newFire.transform.localScale = Scale;
            }

            // 4.3.8 Create all the pedestrians
            foreach (pedestrian Pedestrian in sceneDetails.Pedestrians)
            {
                GameObject newPedestrian = Instantiate(Resources.Load<GameObject>("Prefabs/Pedestrian"));
                Pedestrian pedestrianinfo = newPedestrian.GetComponent<Pedestrian>();

                Transform ExitTransform = null;
                foreach (Transform Object in Scene.transform)
                {
                    if (Object.tag == "Wall")
                    {
                        ExitTransform = Object.Find(AllObjectsNames[Pedestrian.ExitNameIndex]);
                        if (ExitTransform != null)
                        {
                            break;
                        }
                    }
                }
                pedestrianinfo.FillInfo(AllObjectsNames[Pedestrian.NameIndex], Pedestrian.xpos,
                    Pedestrian.ypos, Pedestrian.Speed, Pedestrian.Health, ExitTransform.gameObject);
                newPedestrian.name = pedestrianinfo.Name;
                newPedestrian.transform.SetParent(Scene.transform);

                Vector3 Pos = newPedestrian.transform.position;
                Pos.x = pedestrianinfo.x_pos; Pos.z = pedestrianinfo.y_pos;
                newPedestrian.transform.position = Pos;

                newPedestrian.GetComponent<AICharacterControl>().target = ExitTransform;


            }
            /*
            // 4.3.9 Create the player in InPlayScene
            if (sceneDetails.Players != null)
            {
                foreach (player Player in sceneDetails.Players)
                {
                    GameObject newPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                    Player playerinfo = newPlayer.GetComponent<Player>();
                    playerinfo.FillInfo(AllObjectsNames[Player.NameIndex], Player.xpos, Player.ypos,
                        Player.Speed, Player.Health);
                    newPlayer.name = playerinfo.Name;
                    newPlayer.transform.SetParent(Scene.transform);

                    Vector3 PlayerPos = newPlayer.transform.position;
                    PlayerPos.x = playerinfo.x_pos; PlayerPos.z = playerinfo.y_pos;
                    newPlayer.transform.position = PlayerPos;
                }
            }
            */
        }
    }
}
