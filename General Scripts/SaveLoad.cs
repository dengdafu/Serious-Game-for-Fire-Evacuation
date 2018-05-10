using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {

    public GameObject gamemanager;
    public InputField ScenarioName;
    public Button ScenarioButton;
    private string WorkingDirectory;

    // The following variables are used in DesignScene-related methods
    public GameObject ObjectList;
    public Button WallObjectExampleButton;
    public Button FloorObjectExampleButton;
    public Button CeilingObjectExampleButton;
    public Button ObstacleObjectExampleButton;
    public Button DoorObjectExampleButton;
    public Button FireObjectExampleButton;
    public Button PedestrianObjectExampleButton;

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
            // 4.3.1 Fill in the simulation info
            sceneDetails.SimTime = Scene.GetComponent<SceneInfo>().SimulationTime;
            sceneDetails.TimeStep = Scene.GetComponent<SceneInfo>().TimeStep;
            sceneDetails.GridSize = Scene.GetComponent<SceneInfo>().GridSize;
            // 4.3.2 fill in the info of different objects into sceneDetails
            foreach (Transform ChildObjectTransform in Scene.transform)
            {
                // Wall and its doors
                if (ChildObjectTransform.tag == "Wall")
                {
                    wall WallInfo = new wall();
                    AllObjectNames.Add(ChildObjectTransform.gameObject.name);
                    WallInfo.NameIndex = AllObjectNames.Count;
                    WallInfo.xpos = ChildObjectTransform.position.x;
                    WallInfo.ypos = ChildObjectTransform.position.z;
                    WallInfo.zrot = ChildObjectTransform.eulerAngles.y;
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
        WorkingDirectory = Application.persistentDataPath + "/" + ScenarioButton.name;

        // 2. Get a string[] contains all the subdirectories in the scenario (all the scenes)
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
            Scene.GetComponent<SceneInfo>().TimeStep = sceneDetails.TimeStep;
            Scene.GetComponent<SceneInfo>().GridSize = sceneDetails.GridSize;

            // 4.3.2 Create all the walls
            foreach (wall Wall in sceneDetails.Walls)
            {
                GameObject newWall = Instantiate(Resources.Load<GameObject>("Prefabs/Wall"));
                newWall.name = AllObjectsNames[Wall.NameIndex];
                newWall.transform.SetParent(Scene.transform);

                Vector3 Pos = newWall.transform.position;
                Pos.x = Wall.xpos; Pos.y = Wall.Height / 2; Pos.z = Wall.ypos;
                newWall.transform.position = Pos;

                Vector3 Angles = newWall.transform.eulerAngles;
                Angles.y = Wall.zrot;
                newWall.transform.eulerAngles = Angles;

                Vector3 Scale = newWall.transform.localScale;
                Scale.x = Wall.Width; Scale.y = Wall.Height;
                newWall.transform.localScale = Scale;
                
                Vector4 color = newWall.GetComponent<Renderer>().material.color;
                color[3] = Wall.Opacity;
                newWall.GetComponent<Renderer>().material.color = color;
            }

            // 4.3.3 Create all the floors
            foreach (floor Floor in sceneDetails.Floors)
            {
                GameObject newFloor = Instantiate(Resources.Load<GameObject>("Prefabs/Floor"));
                newFloor.name = AllObjectsNames[Floor.NameIndex];
                newFloor.transform.SetParent(Scene.transform);

                Vector3 Pos = newFloor.transform.position;
                Pos.x = Floor.xpos; Pos.z = Floor.ypos;
                newFloor.transform.position = Pos;

                Vector3 Scale = newFloor.transform.localScale;
                Scale.x = Floor.Width; Scale.z = Floor.Length;
                newFloor.transform.localScale = Scale;
            }

            // 4.3.4 Create all the ceilings
            foreach (ceiling Ceiling in sceneDetails.Ceilings)
            {
                GameObject newCeiling = Instantiate(Resources.Load<GameObject>("Prefabs/Ceiling"));
                newCeiling.name = AllObjectsNames[Ceiling.NameIndex];
                newCeiling.transform.SetParent(Scene.transform);

                Vector3 Pos = newCeiling.transform.position;
                Pos.x = Ceiling.xpos; Pos.y = Ceiling.zpos; Pos.z = Ceiling.ypos;
                newCeiling.transform.position = Pos;

                Vector3 Scale = newCeiling.transform.localScale;
                Scale.x = Ceiling.Width; Scale.z = Ceiling.Length;
                newCeiling.transform.localScale = Scale;

                Vector4 color = newCeiling.GetComponent<Renderer>().material.color;
                color[3] = Ceiling.Opacity;
                newCeiling.GetComponent<Renderer>().material.color = color;
            }

            // 4.3.5 Create all the obstacles
            foreach (obstacle Obstacle in sceneDetails.Obstacles)
            {
                GameObject newObstacle = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"));
                newObstacle.name = AllObjectsNames[Obstacle.NameIndex];
                newObstacle.transform.SetParent(Scene.transform);

                Vector3 Pos = newObstacle.transform.position;
                Pos.x = Obstacle.xpos; Pos.z = Obstacle.ypos;
                newObstacle.transform.position = Pos;

                Vector3 Scale = newObstacle.transform.localScale;
                Scale.x = Obstacle.Width; Scale.y = Obstacle.Height;  Scale.z = Obstacle.Length;
                newObstacle.transform.localScale = Scale;

                Vector4 color = newObstacle.GetComponent<Renderer>().material.color;
                color[3] = Obstacle.Opacity;
                newObstacle.GetComponent<Renderer>().material.color = color;
            }

            // 4.3.6 Create all the doors
            foreach (door Door in sceneDetails.Doors)
            {
                GameObject newDoor = Instantiate(Resources.Load<GameObject>("Prefabs/Door"));
                newDoor.name = AllObjectsNames[Door.NameIndex];

                newDoor.GetComponent<Door>().WallAttachedTo = GameObject.Find(AllObjectsNames[Door.WallNameIndex]);

                newDoor.transform.SetParent(newDoor.GetComponent<Door>().WallAttachedTo.transform);

                foreach (GameObject sceneobj in AllSceneObjects)
                {
                    if (sceneobj.name == AllObjectsNames[Door.SceneNameIndex])
                    {
                        newDoor.GetComponent<Door>().NextScene = sceneobj;
                        break;
                    }
                }

                if (Door.Open == 1)
                {
                    newDoor.GetComponent<Door>().Open = true;
                }
                else
                {
                    newDoor.GetComponent<Door>().Open = false;
                }

                Vector3 Pos = newDoor.transform.localPosition;
                Pos.x = Door.RelativePosition;
                newDoor.transform.localPosition = Pos;

                Vector3 Scale = newDoor.transform.localScale;
                Scale.x = Door.Width; Scale.y = Door.Height;
                newDoor.transform.localScale = Scale;
            }

            // 4.3.7 Create all the fires
            foreach (fire Fire in sceneDetails.Fires)
            {
                GameObject newFire = Instantiate(Resources.Load<GameObject>("Prefabs/Fire"));
                newFire.name = AllObjectsNames[Fire.NameIndex];
                newFire.transform.SetParent(Scene.transform);

                Vector3 Pos = newFire.transform.position;
                Pos.x = Fire.xpos; Pos.y = Fire.zpos; Pos.z = Fire.ypos;
                newFire.transform.position = Pos;

                Vector3 Scale = newFire.transform.localScale;
                Scale.x = Fire.Width; Scale.z = Fire.Length;
                newFire.transform.localScale = Scale;

                newFire.GetComponent<Fire>().HRRPUA = Fire.HRRPUA;
                newFire.GetComponent<Fire>().FUEL = newFire.GetComponent<Fire>().Fuels[Fire.Fuel];
                newFire.GetComponent<Fire>().CO_YIELD = Fire.CO_YIELD;
                newFire.GetComponent<Fire>().SOOT_YIELD = Fire.SOOT_YIELD;
            }

            // 4.3.8 Create all the pedestrians
            foreach (pedestrian Pedestrian in sceneDetails.Pedestrians)
            {
                GameObject newPedestrian = Instantiate(Resources.Load<GameObject>("Prefabs/Pedestrian"));
                newPedestrian.name = AllObjectsNames[Pedestrian.NameIndex];
                newPedestrian.transform.SetParent(Scene.transform);

                Vector3 Pos = newPedestrian.transform.position;
                Pos.x = Pedestrian.xpos; Pos.z = Pedestrian.ypos;
                newPedestrian.transform.position = Pos;

                newPedestrian.GetComponent<Pedestrian>().Speed = Pedestrian.Speed;
                newPedestrian.GetComponent<Pedestrian>().Health = Pedestrian.Health;
                newPedestrian.GetComponent<Pedestrian>().Exit = GameObject.Find(AllObjectsNames[Pedestrian.ExitNameIndex]);
            }
        }
    }

    // DesignSceneSaveOnClick() method is used by the Confirm button in the save panel to save the scenario into a file 
    public void DesignSceneSaveOnClick()
    {
        Save();
    }

    // DesignSceneLoadOnClick() method is used by the Confirm button in the load panel to load the selected scenario
    public void DesignSceneLoadOnClick()
    {
        // When this method is called, the gamemanager is expected to be set to be DesignSceneManager

        // 1. Erase all the GameObjects and their associated buttons too. Therefore, the user should always save the scenario
        // they are working on before loading another scenario.
        foreach (GameObject Scene in gamemanager.GetComponent<DesignSceneGameManager>().AllScenes)
        {
            // For each object in the scene, destory its associated button and then also destory itself.
            foreach(Transform Object in Scene.transform)
            {
                // Destory the associated button
                Destroy(Object.gameObject.GetComponent<AssociatedButton>().button);

                // Destory the game object
                Destroy(Object.gameObject);
            }

            // Then destory the scene (later assign AllScenes in DesignSceneGameManager.cs with the new list of scenes),
            // and its associated button.
            Destroy(Scene.GetComponentInChildren<AssociatedButton>().button);
            Destroy(Scene);
        }

        // 2. Load in the design whose name is ScenarioButton.name
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
                // 3.2.1 Wall buttons
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
                // 3.2.5 Door buttons
                else if (Object.tag == "Door")
                {
                    Button clone = Instantiate(DoorObjectExampleButton);
                    clone.gameObject.SetActive(true);
                    clone.transform.SetParent(ObjectList.transform);
                    clone.GetComponent<RectTransform>().sizeDelta = DoorObjectExampleButton.GetComponent<RectTransform>().sizeDelta;
                    clone.GetComponent<RectTransform>().localEulerAngles = DoorObjectExampleButton.GetComponent<RectTransform>().localEulerAngles;
                    clone.GetComponent<RectTransform>().localPosition = DoorObjectExampleButton.GetComponent<RectTransform>().localPosition;
                    clone.GetComponent<RectTransform>().localScale = DoorObjectExampleButton.GetComponent<RectTransform>().localScale;
                    clone.GetComponent<ObjectButton>().SetLink(Object.gameObject);
                    clone.GetComponentInChildren<Text>().text = Object.gameObject.name;
                    Object.gameObject.GetComponent<AssociatedButton>().button = clone.gameObject;
                }
                // 3.2.6 Fire buttons
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
                // 3.2.7 Pedestrian buttons
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
        }

        // 3.4 Let only the first scene in AllScenes be active.
        // Disable all other scenes and all their related game objects and buttons
        int i = 1;
        do
        {
            foreach (Transform Object in AllScenes[i].transform)
            {
                Object.gameObject.GetComponent<AssociatedButton>().button.SetActive(false);
            }
            AllScenes[i].SetActive(false);
            i++;
        }
        while (i < AllScenes.Count);

        // 3.5 In DesignSceneGameManager, set TempObjectHolder = first scene, CurrentScene = first scene,
        // LastClickedButton = null;
        gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(AllScenes[0]);
        gamemanager.GetComponent<DesignSceneGameManager>().SetCurrentScene(AllScenes[0]);
        gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(null);
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