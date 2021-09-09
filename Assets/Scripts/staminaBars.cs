using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnitySharpNEAT;

public class staminaBars : MonoBehaviour
{

    public BarChart barChart;
    public NeatUI neatCounter;
    public hitCounter uiCounter;
    private GameObject fencer1;
    private GameObject otherfencer1;
    float lastTime = 0f;
    float lastX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();
        neatCounter = GameObject.Find("NeatUI").GetComponent<NeatUI>();
        

        if (barChart == null) // the ChartGraph info is obtained via the inspector
            return;

        float x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        fencer1 = GameObject.FindGameObjectsWithTag("Fencer")[0];
        otherfencer1 = GameObject.FindGameObjectsWithTag("Other Fencer")[0];
        float time = Time.time;

        barChart.DataSource.SetValue("Left Stamina", "Fencers Stamina", otherfencer1.GetComponent<FencerAIController>().currentLeftFencerStamina);
        barChart.DataSource.SetValue("Right Stamina", "Fencers Stamina", fencer1.GetComponent<FencerAIController>().currentRightFencerStamina);

    }
}
