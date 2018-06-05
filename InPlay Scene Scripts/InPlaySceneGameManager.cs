using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;

public class InPlaySceneGameManager : MonoBehaviour {

    public GameObject SmokeParticles;

    public Text time;
    private int currenttime = 0;

    public bool ScenarioLoaded = false;

    public Text InterfaceDensity;
    public Text InterfaceHealthText;
    public GameObject InterfaceHealthBar;

    private void Start()
    {
        // 1. Set WorkingDirectory
        string ScenarioName = ScenarioAtHand.ScenarioName;
        string WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName;

        float WIDTH = 0;
        float LENGTH = 0;
        float HEIGHT = 0;

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
            // 4.3.1 Fill in sim info. If it is in design scene
            Scene.GetComponent<SceneInfo>().SimulationTime = sceneDetails.SimTime;
            Scene.GetComponent<SceneInfo>().Width = sceneDetails.Width;
            Scene.GetComponent<SceneInfo>().Length = sceneDetails.Length;
            Scene.GetComponent<SceneInfo>().Height = sceneDetails.Height;
            Scene.GetComponent<SceneInfo>().PlayerX = sceneDetails.PlayerX;
            Scene.GetComponent<SceneInfo>().PlayerY = sceneDetails.PlayerY;
            // 4.3.2 Fill in the TimeDensity in SceneInfo
            //       and then effectivetimedensity
            if (Directory.Exists(WorkingDirectory + "/TimeDensity/"))
            {
                BinaryFormatter TimeDensitybf = new BinaryFormatter();
                FileStream TimeDensityfile = File.Open(WorkingDirectory + "/TimeDensity/" + "AllData.dat", FileMode.Open);
                List<float> newTimeDensity = (List<float>)TimeDensitybf.Deserialize(TimeDensityfile);
                TimeDensityfile.Close();
                Scene.GetComponent<SceneInfo>().TimeDensity = newTimeDensity;
                // Fill in EffectiveTimeDensity for model1, density at height 1.8m
                List<float> newEfftiveTimeDensity = new List<float>();
                int Width = Mathf.RoundToInt(sceneDetails.Width);
                int Length = Mathf.RoundToInt(sceneDetails.Length);
                int Height = Mathf.RoundToInt(sceneDetails.Height);
                int Time = Mathf.RoundToInt(sceneDetails.SimTime);
                for (int i = 0; i < (Width + 1) * (Length + 1) * Time; i++)
                {
                    newEfftiveTimeDensity.Add(newTimeDensity[1 + i * Height]);
                }

                Scene.GetComponent<SceneInfo>().EffectiveTimeDensity = newEfftiveTimeDensity;
            }

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
                Pos.x = wallinfo.x_pos; Pos.y = wallinfo.Height / 2 + 0.01f; Pos.z = wallinfo.y_pos;
                newWall.transform.position = Pos;

                HEIGHT = wallinfo.Height;

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
                WIDTH = floorinfo.Width; LENGTH = floorinfo.Length;

                Vector3 Pos = newFloor.transform.position;
                Pos.x = floorinfo.x_pos - (Scale.x / 2); Pos.z = floorinfo.y_pos - (Scale.z / 2);
                newFloor.transform.position = Pos;

                // Bake the floor
                newFloor.GetComponent<NavMeshLink>().startPoint = new Vector3(0, 0, 0);
                newFloor.GetComponent<NavMeshLink>().endPoint = new Vector3(Scale.x, 0, Scale.z);
                newFloor.GetComponent<NavMeshSurface>().BuildNavMesh();
            }

            // ADD a light bulb to the scene
            GameObject lightbulb = new GameObject("Scene light");
            lightbulb.AddComponent<Light>();
            lightbulb.GetComponent<Light>().type = LightType.Point;
            lightbulb.transform.SetParent(Scene.transform);
            Vector3 ScenePos = Scene.transform.localPosition;
            lightbulb.transform.localPosition = new Vector3(ScenePos.x, HEIGHT - 0.1f, ScenePos.z);

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
                GameObject newFire = Instantiate(Resources.Load<GameObject>("Prefabs/InPlayFire"));
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

                // For now, set agent's speed to be 0.45 ~ 1.5m/s
                newPedestrian.GetComponent<NavMeshAgent>().speed = 0.45f;
            }
            
            // 4.3.9 Create the player in InPlayScene, the speed is set to be 2 in
            // the inspector for now
            foreach (player Player in sceneDetails.Players)
            {
                GameObject newPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                Player playerinfo = newPlayer.GetComponent<Player>();
                playerinfo.FillInfo(AllObjectsNames[Player.NameIndex], Player.xpos, Player.ypos,
                    Player.Speed, Player.Health);
                newPlayer.name = playerinfo.Name;
                newPlayer.transform.SetParent(Scene.transform);

                Vector3 PlayerPos = newPlayer.transform.position;
                PlayerPos.x = sceneDetails.PlayerX; PlayerPos.z = sceneDetails.PlayerY;
                newPlayer.transform.position = PlayerPos;

                // Put the main camera on it
                GameObject maincamera = GameObject.FindGameObjectWithTag("MainCamera");
                maincamera.transform.SetParent(newPlayer.transform);
                maincamera.transform.localPosition = new Vector3(0, 0, 0);
            }

            // 5. Only activate the scene where the player is in
            if (sceneDetails.Players == null || sceneDetails.Players.Count == 0)
            {
                Scene.SetActive(false);
            }

            // temporary solution
            if (Scene.name == "Outside")
            {
                Scene.SetActive(false);
            }

            // 6. Create all the smoke particles (particle system)
            List<float> timedensity = Scene.GetComponent<SceneInfo>().TimeDensity;
            if (timedensity != null)
            {
                float d = 1;
                float t = 1;
                float T = sceneDetails.SimTime;
                float maxdensity = Mathf.Max(timedensity.ToArray());
                int xnumber = (int)((WIDTH / d) + 1);
                int ynumber = (int)((LENGTH / d) + 1);
                int znumber = (int)((HEIGHT / d));
                int tnumber = (int)(T / t);
                for (int x = 0; x < xnumber; x++)
                {
                    for (int y = 0; y < ynumber; y++)
                    {
                        for (int z = 0; z < znumber; z++)
                        {
                            List<float> smokedensity = new List<float>();
                            for (int n = 0; n < tnumber; n++)
                            {
                                int index;
                                index = n * (xnumber * ynumber * znumber) +
                                        y * (xnumber * znumber) +
                                        x * znumber + z;
                                smokedensity.Add(timedensity[index]);
                            }
                            if (Mathf.Max(smokedensity.ToArray()) > (maxdensity / 10))
                            {
                                GameObject newSmoke = Instantiate(Resources.Load<GameObject>("Prefabs/Smoke"));
                                newSmoke.GetComponent<InPlaySmoke>().MaxDensity = maxdensity;
                                newSmoke.GetComponent<InPlaySmoke>().Density = smokedensity;
                                newSmoke.transform.SetParent(Scene.transform);
                                newSmoke.transform.localPosition =
                                    new Vector3(-(WIDTH / 2) + x * d, 0.5f * d + z * d, -(LENGTH / 2) + y * d);
                            }
                        }
                    }
                }
            }

            // 7. Set currenttime to be 0
            currenttime = 0;
        }
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad >= currenttime)
        {
            time.text = currenttime.ToString();
            currenttime += 1;
        }
    }
}
