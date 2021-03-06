using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnitySharpNEAT;

public class hitBarChart : MonoBehaviour
{
    public BarChart barChart;
    public NeatUI neatCounter;
    public hitCounter uiCounter;
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
        
        float allLeftHit = uiCounter.allLeftHit;
        float allRightHit = uiCounter.allRightHit;
        float allLeftDodges = uiCounter.allLeftDodges;
        float allRightDodges = uiCounter.allRightDodges;

        /*
        float currentleftZoneLeftFencer = neatCounter.currentLeftZoneLeftFencer;
        float currentrightZoneLeftFencer = neatCounter.currentRightZoneLeftFencer;
        float currentleftZoneRightFencer = neatCounter.currentLeftZoneRightFencer;
        float currentrightZoneRightFencer = neatCounter.currentRightZoneRightFencer;
        */
        float time = Time.time;
        
            barChart.DataSource.SetValue("Left Hits", "Hits and Dodges", allLeftHit);
            barChart.DataSource.SetValue("Left Dodges", "Hits and Dodges", allLeftDodges);
            barChart.DataSource.SetValue("Right Hits", "Hits and Dodges", allRightHit);
            barChart.DataSource.SetValue("Right Dodges", "Hits and Dodges", allRightDodges);
        
    }
}
