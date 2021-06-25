using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnitySharpNEAT;

public class fitnessChart : MonoBehaviour
{

    // Start is called before the first frame update
    public GraphChart Graph;
    public NeatUI neatCounter;


    public int TotalPoints = 5;
    float lastTime = 0f;
    float lastX = 0f;

    void Start()
    {

        neatCounter = GameObject.Find("NeatUI").GetComponent<NeatUI>();
        
        if (Graph == null) // the ChartGraph info is obtained via the inspector
            return;
        float x = 0f;
     /////   Graph.DataSource.StartBatch(); // do not call StartBatch for realtime calls , it will only slow down performance.
 
        Graph.DataSource.ClearCategory("Player 1"); // clear the "Player 1" category. this category is defined using the GraphChart inspector
        Graph.DataSource.ClearCategory("Player 2"); // clear the "Player 2" category. this category is defined using the GraphChart inspector

        /*
        for (int i = 0; i < TotalPoints; i++)  //add random points to the graph
        {
            Graph.DataSource.AddPointToCategoryRealtime("Player 1", x, Random.value * 20f + 10f); // each time we call AddPointToCategory 
            Graph.DataSource.AddPointToCategoryRealtime("Player 2", x, Random.value * 10f); // each time we call AddPointToCategory 
            x += Random.value * 3f;
            lastX = x;

        }
        */
      ////  Graph.DataSource.EndBatch(); // do not batch reatlime calls
    }

    // Update is called once per frame
    void Update()
    {
        float currentGen = neatCounter.currentGen;
        float lastGen = neatCounter.lastGen;
        float time = Time.time;
        
        if (lastTime + 2f < time)
        {
            lastTime = time;
            lastX += Random.value * 3f;
            Graph.DataSource.AddPointToCategoryRealtime("Player 1", lastX, neatCounter.leftFit); // each time we call AddPointToCategory 
            Graph.DataSource.AddPointToCategoryRealtime("Player 2", lastX, neatCounter.rightFit); // each time we call AddPointToCategory
        }
        
       

    }
}
