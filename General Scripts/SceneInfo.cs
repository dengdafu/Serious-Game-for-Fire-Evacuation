using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour {

    public float SimulationTime = 0;

    public float Width = 0; //X
    public float Length = 0; //Y
    public float Height = 0; //Z

    // Player spawning position
    public float PlayerX;
    public float PlayerY;

    public List<float> TimeDensity;
    public List<float> EffectiveTimeDensity; // for Model1
}
