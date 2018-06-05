using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Model1 : MonoBehaviour {

    List<float> EffectiveTimeDensity;
    float MaxDensity;
    Pedestrian PedestrianInfo = null;
    Player PlayerInfo = null;

    List<GameObject> InRangeWalls;
    List<GameObject> InRangeObstacles;
    List<GameObject> InRangeAgents;
    List<GameObject> InRangePlayers;

    float SimTime;
    float UpdateTimeStep = 0.1f;
    int time_count = 0;
    int SmokeToHumanTime = 0;
    float current_time;
    int domainWidth;
    int domainLength;

    // for player
    public Text DensityText;
    public Text HealthText;
    public GameObject HealthBar;

    // Use this for initialization
    void Start () {
        SceneInfo thisSceneInfo = this.gameObject.transform.parent.GetComponent<SceneInfo>();
        EffectiveTimeDensity = thisSceneInfo.EffectiveTimeDensity;
        MaxDensity = Mathf.Max(EffectiveTimeDensity.ToArray());
        current_time = 0;
        domainWidth = Mathf.RoundToInt(thisSceneInfo.Width);
        domainLength = Mathf.RoundToInt(thisSceneInfo.Length);
        SimTime = thisSceneInfo.SimulationTime;
        InRangeWalls = new List<GameObject>();
        InRangeObstacles = new List<GameObject>();
        InRangeAgents = new List<GameObject>();
        InRangePlayers = new List<GameObject>();

        if (this.gameObject.tag == "Player")
        {
            InPlaySceneGameManager gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InPlaySceneGameManager>();
            if (DensityText == null)
            {
                DensityText = gamemanager.InterfaceDensity;
            }
            if (HealthText == null)
            {
                HealthText = gamemanager.InterfaceHealthText;
            }
            if (HealthBar == null)
            {
                HealthBar = gamemanager.InterfaceHealthBar;
            }
        }
	}

    // Keep track what is in the active range.
    // active range is set to be 3 by setting the sphere collider's radius to be 3
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            InRangeWalls.Add(other.gameObject);
        }
        else if (other.tag == "Obstacle")
        {
            InRangeObstacles.Add(other.gameObject);
        }
        else if (other.tag == "Pedestrian")
        {
            InRangeAgents.Add(other.gameObject);
        }
        else if (other.tag == "Player")
        {
            InRangePlayers.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            InRangeWalls.Remove(other.gameObject);
        }
        else if (other.tag == "Obstacle")
        {
            InRangeObstacles.Remove(other.gameObject);
        }
        else if (other.tag == "Pedestrian")
        {
            InRangeAgents.Remove(other.gameObject);
        }
        else if (other.tag == "Player")
        {
            InRangePlayers.Remove(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        current_time = Time.timeSinceLevelLoad;
        if (current_time > time_count * UpdateTimeStep)
        {
            time_count += 1;
            SmokeToHuman();
        }
		
	}

    // Smoke Human Interaction
    void SmokeToHuman()
    {
        Vector3 position = this.gameObject.transform.position;
        int X = Mathf.RoundToInt(position.x) + (domainWidth / 2);
        int Y = Mathf.RoundToInt(position.z) + (domainLength / 2);
        int T = SmokeToHumanTime;
        if (current_time >= SmokeToHumanTime + 1)
        {
            SmokeToHumanTime += 1;
        }
        int index = ((domainWidth + 1) * (domainLength + 1)) * T +
                    (domainWidth + 1) * Y + X;

        if (T <= SimTime)
        {
            float Density = EffectiveTimeDensity[index];
            // Then use this density to do sth
            float damage = (Density / MaxDensity) * 10; // Let's set damage in this way for now
            // if it is a player:
            // 1. reduce its health
            // 2. change the interface accordingly
            if (this.tag == "Player")
            {
                DensityText.text = Density.ToString();
                Player PlayerInfo = this.GetComponent<Player>();
                PlayerInfo.Health -= damage;

                // if dead, go to evaluation scene;
                if (PlayerInfo.Health <= 0)
                {
                    PlayerDeath();
                }
                HealthText.text = PlayerInfo.Health.ToString();
                HealthBar.transform.localScale = new Vector3(PlayerInfo.Health / 100, 1, 1);
            }
            else if (this.tag == "Pedestrian")
            {
                Pedestrian PedInfo = this.GetComponent<Pedestrian>();
                PedInfo.Health -= damage;

                // if dead, destroy the pedestrian and maybe do something else
                if (PedInfo.Health <= 0)
                {
                    PedestrianDeath();
                }
            }
        }
    }
    void PlayerDeath()
    {
        SceneManager.LoadScene("Evaluation scene");
    }
    void PedestrianDeath()
    {
        Destroy(this.gameObject);
    }

    // Obstacle Human Interaction
    void ObstacleToHuman()
    {

    }

}
