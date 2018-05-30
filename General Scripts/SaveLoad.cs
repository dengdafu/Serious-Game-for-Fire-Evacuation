using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {

    public GameObject gamemanager;
    public InputField ScenarioName;
    private string WorkingDirectory;

    public GameObject WarningWindow;

    // The following variables are used in DesignScene-related methods
    public GameObject ObjectList;
    public Button WallObjectExampleButton;
    public Button FloorObjectExampleButton;
    public Button CeilingObjectExampleButton;
    public Button ObstacleObjectExampleButton;
    public Button DoorObjectExampleButton;
    public Button FireObjectExampleButton;
    public Button PedestrianObjectExampleButton;
    public Button PlayerObjectExampleButton;

    public GameObject SceneList;
    public GameObject SceneButtonInstance;

    public void Save()
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
        // If there already exists a scenario with scenarioName, replace it with the new one. Therefore,
        // users should make sure not to use the same scenario name
        if (Directory.Exists(WorkingDirectory))
        {
            Directory.Delete(WorkingDirectory, true);
        }
        Directory.CreateDirectory(WorkingDirectory);

        // 4. For each scene, do the followings
        foreach (GameObject Scene in AllScenes)
        {
            // 4.1 create a sub-folder with the scenarioName name
            WorkingDirectory = Application.persistentDataPath + "/" + scenarioName +  "/" + Scene.name;
            Directory.CreateDirectory(WorkingDirectory);

            // 4.2 create a list to store all the names of the objects
            List<string> AllObjectNames = new List<string>();

            // 4.3 Fill in the class SceneDetails according to the design of the scene
            SceneDetails sceneDetails = new SceneDetails();
            sceneDetails.Walls = new List<wall>();
            sceneDetails.Floors = new List<floor>();
            sceneDetails.Ceilings = new List<ceiling>();
            sceneDetails.Obstacles = new List<obstacle>();
            sceneDetails.Doors = new List<door>();
            sceneDetails.Fires = new List<fire>();
            sceneDetails.Pedestrians = new List<pedestrian>();
            sceneDetails.Players = new List<player>();
            // 4.3.1 Fill in the simulation info
            sceneDetails.SimTime = Scene.GetComponent<SceneInfo>().SimulationTime;
            sceneDetails.Width = Scene.GetComponent<SceneInfo>().Width;
            sceneDetails.Length = Scene.GetComponent<SceneInfo>().Length;
            sceneDetails.Height = Scene.GetComponent<SceneInfo>().Height;
            sceneDetails.PlayerX = Scene.GetComponent<SceneInfo>().PlayerX;
            sceneDetails.PlayerY = Scene.GetComponent<SceneInfo>().PlayerY;
            // 4.3.2 fill in the info of different objects into sceneDetails
            foreach (Transform ChildObjectTransform in Scene.transform)
            {
                // Wall and its doors
                if (ChildObjectTransform.tag == "Wall")
                {
                    wall WallInfo = new wall();
                    Wall wallinfo = ChildObjectTransform.gameObject.GetComponent<Wall>();
                    AllObjectNames.Add(wallinfo.Name);
                    WallInfo.NameIndex = AllObjectNames.Count - 1;
                    WallInfo.xpos = wallinfo.x_pos;
                    WallInfo.ypos = wallinfo.y_pos;
                    WallInfo.zrot = wallinfo.z_rot;
                    WallInfo.Width = wallinfo.Width;
                    WallInfo.Height = wallinfo.Height;
                    WallInfo.Opacity = wallinfo.Opacity;
                    sceneDetails.Walls.Add(WallInfo);

                    // doors that are attached to it
                    foreach (Transform doorTransform in ChildObjectTransform)
                    {
                        if (doorTransform.tag == "Door")
                        {
                            door DoorInfo = new door();
                            Door doorinfo = doorTransform.gameObject.GetComponent<Door>();
                            AllObjectNames.Add(doorinfo.Name);
                            DoorInfo.NameIndex = AllObjectNames.Count - 1;
                            DoorInfo.RelativePosition = doorinfo.RelativePosition;
                            DoorInfo.Width = doorinfo.Width;
                            DoorInfo.Height = doorinfo.Height;
                            if (doorinfo.Open)
                            {
                                DoorInfo.Open = 1;
                            }
                            else
                            {
                                DoorInfo.Open = 0;
                            }
                            DoorInfo.WallNameIndex = WallInfo.NameIndex;
                            AllObjectNames.Add(doorinfo.NextScene.name);
                            DoorInfo.SceneNameIndex = AllObjectNames.Count - 1;
                            sceneDetails.Doors.Add(DoorInfo);
                        }
                    }
                }
                // Floor
                else if (ChildObjectTransform.tag == "Floor")
                {
                    floor FloorInfo = new floor();
                    Floor floorinfo = ChildObjectTransform.gameObject.GetComponent<Floor>();
                    AllObjectNames.Add(floorinfo.Name);
                    FloorInfo.NameIndex = AllObjectNames.Count - 1;
                    FloorInfo.xpos = floorinfo.x_pos;
                    FloorInfo.ypos = floorinfo.y_pos;
                    FloorInfo.Width = floorinfo.Width;
                    FloorInfo.Length = floorinfo.Length;
                    sceneDetails.Floors.Add(FloorInfo);
                }
                // Ceiling
                else if (ChildObjectTransform.tag == "Ceiling")
                {
                    ceiling CeilingInfo = new ceiling();
                    Ceiling ceilinginfo = ChildObjectTransform.gameObject.GetComponent<Ceiling>();
                    AllObjectNames.Add(ceilinginfo.Name);
                    CeilingInfo.NameIndex = AllObjectNames.Count - 1;
                    CeilingInfo.xpos = ceilinginfo.x_pos;
                    CeilingInfo.ypos = ceilinginfo.y_pos;
                    CeilingInfo.zpos = ceilinginfo.z_pos;
                    CeilingInfo.Width = ceilinginfo.Width;
                    CeilingInfo.Length = ceilinginfo.Length;
                    CeilingInfo.Opacity = ceilinginfo.Opacity;
                    sceneDetails.Ceilings.Add(CeilingInfo);
                }
                // Obstacle
                else if (ChildObjectTransform.tag == "Obstacle")
                {
                    obstacle ObstacleInfo = new obstacle();
                    Obstacle obstacleinfo = ChildObjectTransform.gameObject.GetComponent<Obstacle>();
                    AllObjectNames.Add(obstacleinfo.Name);
                    ObstacleInfo.NameIndex = AllObjectNames.Count - 1;
                    ObstacleInfo.xpos = obstacleinfo.x_pos;
                    ObstacleInfo.ypos = obstacleinfo.y_pos;
                    ObstacleInfo.Width = obstacleinfo.Width;
                    ObstacleInfo.Length = obstacleinfo.Length;
                    ObstacleInfo.Height = obstacleinfo.Height;
                    ObstacleInfo.Opacity = obstacleinfo.Opacity;
                    sceneDetails.Obstacles.Add(ObstacleInfo);
                }
                // Fire
                else if (ChildObjectTransform.tag == "Fire")
                {
                    fire FireInfo = new fire();
                    Fire fireinfo = ChildObjectTransform.gameObject.GetComponent<Fire>();
                    AllObjectNames.Add(fireinfo.Name);
                    FireInfo.NameIndex = AllObjectNames.Count - 1;
                    FireInfo.xpos = fireinfo.x_pos;
                    FireInfo.ypos = fireinfo.y_pos;
                    FireInfo.zpos = fireinfo.z_pos;
                    FireInfo.Width = fireinfo.Width;
                    FireInfo.Length = fireinfo.Length;
                    FireInfo.HRRPUA = fireinfo.HRRPUA;
                    FireInfo.CO_YIELD = fireinfo.CO_YIELD;
                    FireInfo.SOOT_YIELD = fireinfo.SOOT_YIELD;
                    FireInfo.Fuel = Array.IndexOf(fireinfo.Fuels,
                        fireinfo.FUEL);
                    sceneDetails.Fires.Add(FireInfo);
                }
                // Pedestrian
                else if (ChildObjectTransform.tag == "Pedestrian")
                {
                    pedestrian PedestrianInfo = new pedestrian();
                    Pedestrian pedestrianinfo = ChildObjectTransform.gameObject.GetComponent<Pedestrian>();
                    AllObjectNames.Add(pedestrianinfo.Name);
                    PedestrianInfo.NameIndex = AllObjectNames.Count - 1;
                    PedestrianInfo.xpos = pedestrianinfo.x_pos;
                    PedestrianInfo.ypos = pedestrianinfo.y_pos;
                    PedestrianInfo.Speed = pedestrianinfo.Speed;
                    PedestrianInfo.Health = pedestrianinfo.Health;
                    AllObjectNames.Add(pedestrianinfo.Exit.name);
                    PedestrianInfo.ExitNameIndex = AllObjectNames.Count - 1;
                    sceneDetails.Pedestrians.Add(PedestrianInfo);
                }
                else if (ChildObjectTransform.tag == "Player")
                {
                    player PlayerInfo = new player();
                    Player playerinfo = ChildObjectTransform.gameObject.GetComponent<Player>();
                    AllObjectNames.Add(playerinfo.Name);
                    PlayerInfo.NameIndex = AllObjectNames.Count - 1;
                    PlayerInfo.xpos = playerinfo.x_pos;
                    PlayerInfo.ypos = playerinfo.y_pos;
                    PlayerInfo.Speed = playerinfo.Speed;
                    PlayerInfo.Health = playerinfo.Health;
                    sceneDetails.Players.Add(PlayerInfo);
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
            FileStream file = File.Open(WorkingDirectory + "/SceneDetails.dat", FileMode.OpenOrCreate);
            bf.Serialize(file, sceneDetails);
            file.Close();
        }
    }

    // Method LoadOnClick() only bring back the stored objects. Extra steps need to be taken to recover to design scene
    // and game scene accordingly.
    public void Load()
    {
        // 1. Set WorkingDirectory
        WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName.text;

        // 2. Get a List<string> contains all the subdirectories in the scenario (all the scenes)
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
            WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName.text + "/" + Scene.name;
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
            Scene.GetComponent<SceneInfo>().Width = sceneDetails.Width;
            Scene.GetComponent<SceneInfo>().Length = sceneDetails.Length;
            Scene.GetComponent<SceneInfo>().Height = sceneDetails.Height;
            Scene.GetComponent<SceneInfo>().PlayerX = sceneDetails.PlayerX;
            Scene.GetComponent<SceneInfo>().PlayerY = sceneDetails.PlayerY;

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
                GameObject newFloor = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"));
                Floor floorinfo = newFloor.GetComponent<Floor>();
                floorinfo.FillInfo(AllObjectsNames[Floor.NameIndex], Floor.xpos, Floor.ypos, Floor.Length,
                    Floor.Width);
                newFloor.name = floorinfo.Name;
                newFloor.transform.SetParent(Scene.transform);

                Vector3 Pos = newFloor.transform.position;
                Pos.x = floorinfo.x_pos; Pos.z = floorinfo.y_pos;
                newFloor.transform.position = Pos;

                Vector3 Scale = newFloor.transform.localScale;
                Scale.x = floorinfo.Width; Scale.z = floorinfo.Length;
                newFloor.transform.localScale = Scale;
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

                Vector4 color = newCeiling.GetComponent<Renderer>().material.color;
                color[3] = ceilinginfo.Opacity;
                newCeiling.GetComponent<Renderer>().material.color = color;
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
                Scale.x = obstacleinfo.Width; Scale.y = obstacleinfo.Height;  Scale.z = obstacleinfo.Length;
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
                GameObject newPedestrian = Instantiate(Resources.Load<GameObject>("Prefabs/DesignScenePedestrian"));
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
            }

            // 4.3.9 Create the player
            if (sceneDetails.Players != null)
            {
                foreach (player Player in sceneDetails.Players)
                {
                    GameObject newPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/DesignScenePlayer"));
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
        }
    }

    // DesignSceneSaveOnClick() method is used by the Confirm button in the save panel to save the scenario into a file 
    public void DesignSceneSaveOnClick()
    {
        Save();

        WarningWindow.SetActive(true);
        WarningWindow.GetComponentInChildren<Text>().text = "Saved successfully!";
    }

    // DesignSceneLoadOnClick() method is used by the Confirm button in the load panel to load the selected scenario
    public void DesignSceneLoadOnClick()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + ScenarioName.text))
        {
            WarningWindow.SetActive(true);
            WarningWindow.GetComponentInChildren<Text>().text = "No such file!";
        }

        else
        {
            // When this method is called, the gamemanager is expected to be set to be DesignSceneManager

            // 1. Erase all the GameObjects and their associated buttons too. Therefore, the user should always save the scenario
            // they are working on before loading another scenario.
            foreach (GameObject Scene in gamemanager.GetComponent<DesignSceneGameManager>().AllScenes)
            {
                // For each object in the scene, destory its associated button and then also destory itself.
                foreach (Transform Object in Scene.transform)
                {
                    if (Object.tag == "Wall")
                    {
                        foreach (Transform door in Object.transform)
                        {
                            Destroy(door.gameObject.GetComponent<AssociatedButton>().button);
                            Destroy(Object.gameObject);
                        }
                    }
                    // Destory the associated button
                    Destroy(Object.gameObject.GetComponent<AssociatedButton>().button);

                    // Destory the game object
                    Destroy(Object.gameObject);
                }

                // Then destory the scene (later assign AllScenes in DesignSceneGameManager.cs with the new list of scenes),
                // and its associated button.
                Scene.SetActive(false); //This line will make it work, not sure why...
                Destroy(Scene.GetComponent<AssociatedButton>().button);
                Destroy(Scene);
            }

            // 2. Load in the design whose name is ScenarioName.text
            Load();

            // 3. Fill in the variables in DesignSceneGameManager.cs based on the loaded scene
            // 3.1 Fill in AllScenes
            List<GameObject> AllScenes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Scene"));
            gamemanager.GetComponent<DesignSceneGameManager>().AllScenes = AllScenes;
            // 3.2 For each game object in each scene, create button for it
            foreach (GameObject Scene in AllScenes)
            {
                foreach (Transform Object in Scene.transform)
                {
                    // 3.2.1 Wall buttons and buttons for their doors
                    if (Object.tag == "Wall")
                    {
                        Button clone = Instantiate(WallObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = WallObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = WallObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = WallObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = WallObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;

                        foreach (Transform door in Object)
                        {
                            Button doorclone = Instantiate(DoorObjectExampleButton);
                            doorclone.gameObject.SetActive(true);
                            doorclone.transform.SetParent(ObjectList.transform);
                            doorclone.GetComponent<RectTransform>().sizeDelta = DoorObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                            doorclone.GetComponent<RectTransform>().localEulerAngles = DoorObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                            doorclone.GetComponent<RectTransform>().localPosition = DoorObjectExampleButton.GetComponent<RectTransform>().localPosition;
                            doorclone.GetComponent<RectTransform>().localScale = DoorObjectExampleButton.GetComponent<RectTransform>().localScale;
                            doorclone.GetComponent<ObjectButton>().SetLink(door.gameObject);
                            doorclone.GetComponentInChildren<Text>().text = door.gameObject.name;
                            door.gameObject.GetComponent<AssociatedButton>().button = doorclone.gameObject;
                        }

                    }
                    // 3.2.2 Floor buttons
                    else if (Object.tag == "Floor")
                    {
                        Button clone = Instantiate(FloorObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = FloorObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = FloorObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = FloorObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = FloorObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                    // 3.2.3 Ceiling buttons
                    else if (Object.tag == "Ceiling")
                    {
                        Button clone = Instantiate(CeilingObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = CeilingObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = CeilingObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = CeilingObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = CeilingObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                    // 3.2.4 Obstacle buttons
                    else if (Object.tag == "Obstacle")
                    {
                        Button clone = Instantiate(ObstacleObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = ObstacleObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = ObstacleObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = ObstacleObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = ObstacleObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                    // 3.2.5 Fire buttons
                    else if (Object.tag == "Fire")
                    {
                        Button clone = Instantiate(FireObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = FireObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = FireObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = FireObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = FireObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                    // 3.2.6 Pedestrian buttons
                    else if (Object.tag == "Pedestrian")
                    {
                        Button clone = Instantiate(PedestrianObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = PedestrianObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = PedestrianObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = PedestrianObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = PedestrianObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                    // 3.2.7 Player buttons
                    else if (Object.tag == "Player")
                    {
                        Button clone = Instantiate(PlayerObjectExampleButton);
                        clone.gameObject.SetActive(true);
                        clone.transform.SetParent(ObjectList.transform);
                        clone.GetComponent<RectTransform>().sizeDelta = PlayerObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                        clone.GetComponent<RectTransform>().localEulerAngles = PlayerObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                        clone.GetComponent<RectTransform>().localPosition = PlayerObjectExampleButton.GetComponent<RectTransform>().localPosition;
                        clone.GetComponent<RectTransform>().localScale = PlayerObjectExampleButton.GetComponent<RectTransform>().localScale;
                        clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                        clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                        Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                    }
                }
            }

            // 3.3 Create button for all scenes, also attach AssociatedButton.cs to the scene object
            foreach (GameObject Scene in AllScenes)
            {
                // Attach AssociatedButton.cs to it
                Scene.AddComponent<AssociatedButton>();

                GameObject NewSceneButton = Instantiate(SceneButtonInstance);
                NewSceneButton.SetActive(true);
                NewSceneButton.transform.SetParent(SceneList.transform);
                NewSceneButton.GetComponentInChildren<Text>().text = Scene.name;
                NewSceneButton.transform.position = SceneButtonInstance.transform.position;
                NewSceneButton.transform.localEulerAngles = SceneButtonInstance.transform.localEulerAngles;
                NewSceneButton.transform.localScale = SceneButtonInstance.transform.localScale;
                Scene.GetComponent<AssociatedButton>().button = NewSceneButton;
                NewSceneButton.GetComponent<SceneButton>().LinkedGameObject = Scene;
            }

            // 3.4 Let only the first scene in AllScenes be active.
            // Disable all other scenes and all their related game objects and buttons
            if (AllScenes.Count != 1)
            {
                int i = 1;
                do
                {
                    foreach (Transform Object in AllScenes[i].transform)
                    {
                        if (Object.tag == "Wall")
                        {
                            foreach (Transform door in Object.transform)
                            {
                                door.gameObject.GetComponent<AssociatedButton>().button.SetActive(false);
                            }
                        }
                        Object.gameObject.GetComponent<AssociatedButton>().button.SetActive(false);
                    }
                    AllScenes[i].SetActive(false);
                    i++;
                }
                while (i < AllScenes.Count);
            }

            // 3.5 In DesignSceneGameManager, set TempObjectHolder = first scene, CurrentScene = first scene,
            // LastClickedButton = null; Update the Header
            gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(AllScenes[0]);
            gamemanager.GetComponent<DesignSceneGameManager>().SetCurrentScene(AllScenes[0]);
            gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
            gamemanager.GetComponent<DesignSceneGameManager>().Header.GetComponentInChildren<Text>().text = AllScenes[0].name;
        }
    }

    // ExistingFilesOnClick() method is used in the design scene -> load panel to bring up all the existing files
    public void ExisingFilesOnClick()
    {
        WarningWindow.SetActive(true);
        var info = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] Directories = info.GetDirectories();
        string directories = "";
        foreach (DirectoryInfo directory in Directories)
        {
            if (directory.Name != "Unity" & directory.Name != "Crashes")
            {
                directories += directory.Name;
                directories += ", ";
            }
        }
        WarningWindow.GetComponentInChildren<Text>().text = directories;
    }
}

[Serializable]
public class SceneDetails
{
    public float SimTime;
    public float Width;
    public float Length;
    public float Height;
    public float PlayerX;
    public float PlayerY;
    public List<wall> Walls;
    public List<floor> Floors;
    public List<ceiling> Ceilings;
    public List<obstacle> Obstacles;
    public List<door> Doors;
    public List<fire> Fires;
    public List<pedestrian> Pedestrians;
    public List<player> Players;
}

[Serializable]
public class wall
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
public class floor
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float Width;
    public float Length;
}

[Serializable]
public class ceiling
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float zpos;
    public float Opacity;
    public float Width;
    public float Length;
}

[Serializable]
public class obstacle
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
public class door
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
public class fire
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
public class pedestrian
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public int ExitNameIndex;
    public float Speed;
    public float Health;
}

[Serializable]
public class player
{
    public int NameIndex;
    public float xpos;
    public float ypos;
    public float Speed;
    public float Health;
}