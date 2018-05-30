using System.Collections.Generic;
using UnityEngine;

public class InPlaySmoke : MonoBehaviour {

    public float MaxDensity;
    public List<float> Density;

    public int timecount = 0;
    public float current_time = 0;
    float opacity;
    Color newcolor;

    private void Start()
    {
        current_time = 0;
    }

    void Update () {
        current_time = Time.timeSinceLevelLoad;
        if (current_time > timecount)
        {
            if (timecount < Density.Count)
            {
                ParticleSystem.MainModule main = this.gameObject.GetComponent<ParticleSystem>().main;
                opacity = (Density[timecount] / MaxDensity) * 0.3f;
                if (opacity < 0)
                {
                    newcolor = new Color(1, 1, 1, 0);
                }
                else
                {
                    newcolor = new Color(1, 1, 1, opacity);
                }
                main.startColor = new ParticleSystem.MinMaxGradient(newcolor);
            }
            timecount += 1;
        }
    }
}
