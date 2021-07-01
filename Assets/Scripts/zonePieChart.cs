using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnitySharpNEAT;

public class zonePieChart : MonoBehaviour
{
    public PieChart pieChart;
    public NeatUI neatCounter;
    public hitCounter uiCounter;
    // Start is called before the first frame update
    void Start()
    {
        uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();
        neatCounter = GameObject.Find("NeatUI").GetComponent<NeatUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float allLeftZoneLeftFencer = uiCounter.allLeftZoneLeftFencer;
        float allLeftZoneRightFencer = uiCounter.allLeftZoneRightFencer;
        float allRightZoneLeftFencer = uiCounter.allRightZoneLeftFencer;
        float allRightZoneRightFencer = uiCounter.allRightZoneRightFencer;

        float currentLeftZoneLeftFencer = neatCounter.currentLeftZoneLeftFencer;
        float currentLeftZoneRightFencer = neatCounter.currentLeftZoneRightFencer;
        float currentRightZoneLeftFencer = neatCounter.currentRightZoneLeftFencer;
        float currentRightZoneRightFencer = neatCounter.currentRightZoneRightFencer;

        pieChart.DataSource.SetValue("L Def", allLeftZoneLeftFencer);
        pieChart.DataSource.SetValue("L Atk", allRightZoneLeftFencer);
        pieChart.DataSource.SetValue("R Def", allRightZoneRightFencer);
        pieChart.DataSource.SetValue("R Atk",  allLeftZoneRightFencer);
    }
}
