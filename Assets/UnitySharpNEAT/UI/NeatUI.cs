/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnitySharpNEAT
{
    public class NeatUI : MonoBehaviour
    {
        [SerializeField] 
        private NeatSupervisor _neatSupervisor;

        [SerializeField]
        public GameObject leftScreen;
        
        [SerializeField]
        private GameObject rightScreen;

        [SerializeField]
        private GameObject leftStaminaScreen;

        [SerializeField]
        private GameObject rightStaminaScreen;

        /// <summary>
        /// Display simple Onscreen buttons for quickly accessing ceratain lifecycle funtions and to display generation info.
        /// </summary>

        public SabreHit sabreHitLeftScripts;
        public SabreHit sabreHitRightScripts;
        public float leftHit;
        public float rightHit;
        public float leftFit = 0;
        public float rightFit = 0;
        public float currentGen = 0;
        public float lastGen = 0;

        public float currentLeftZoneLeftFencer = 0;
        public float currentRightZoneLeftFencer = 0;
        public float currentLeftZoneRightFencer = 0;
        public float currentRightZoneRightFencer = 0;

        public float currentLeftDodges = 0;
        public float currentRightDodges = 0;

        public float currentLeftAttemptedHits = 0;
        public float currentRightAttemptedHits = 0;
        public bool isStart = true;

        public bool isBothHitUI = false;
        hitCounter uiCounter;
        private GameObject fencer1;
        private GameObject otherfencer1;
        
        private void Start(){
            uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();
            //SceneManager.LoadScene("FencingScene3");
            //SceneManager.LoadScene("SampleScene");
        }

        private void OnGUI()
        {
            if(isStart){
                if (GUI.Button(new Rect(10, 10, 110, 40), "Start EA"))
            {
                _neatSupervisor.StartEvolution();

                lastGen = _neatSupervisor.CurrentGeneration;
            }
            }
            

            fencer1 = GameObject.FindGameObjectsWithTag("Fencer")[0];
            sabreHitRightScripts = fencer1.GetComponentInChildren<SabreHit>();
            otherfencer1 = GameObject.FindGameObjectsWithTag("Other Fencer")[0];
            
            sabreHitLeftScripts = otherfencer1.GetComponentInChildren<SabreHit>();
            leftHit = sabreHitLeftScripts.currentLeftHit;
            rightHit = sabreHitRightScripts.currentRightHit;

            currentLeftZoneLeftFencer = otherfencer1.GetComponent<FencerAIController>().currentLeftZoneLeftFencer;
            currentRightZoneLeftFencer = otherfencer1.GetComponent<FencerAIController>().currentRightZoneLeftFencer;
            currentLeftZoneRightFencer = fencer1.GetComponent<FencerAIController>().currentLeftZoneRightFencer;
            currentRightZoneRightFencer = fencer1.GetComponent<FencerAIController>().currentRightZoneRightFencer;

            currentLeftDodges = otherfencer1.GetComponent<FencerAIController>().currentLeftDodges;
            currentRightDodges = fencer1.GetComponent<FencerAIController>().currentRightDodges;
            currentLeftAttemptedHits = otherfencer1.GetComponent<FencerAIController>().currentLeftAttemptedHits;
            currentRightAttemptedHits = fencer1.GetComponent<FencerAIController>().currentRightAttemptedHits;
            
            //sabreHitScripts = GameObject.Find("Sword_blade").GetComponent<SabreHit>();
            
            leftScreen.GetComponent<TextMeshPro>().text = uiCounter.allLeftHit.ToString();
            rightScreen.GetComponent<TextMeshPro>().text = uiCounter.allRightHit.ToString();

            //leftStaminaScreen.GetComponent<TextMeshPro>().text =  string.Format("{0:0.00}", otherfencer1.GetComponent<FencerAIController>().currentLeftFencerStamina);
            //rightStaminaScreen.GetComponent<TextMeshPro>().text = string.Format("{0:0.00}", fencer1.GetComponent<FencerAIController>().currentRightFencerStamina);

            if(uiCounter.allLeftHit > uiCounter.allRightHit) {
                leftScreen.GetComponent<TextMeshPro>().color = new Color32(250, 204, 16, 255);
                rightScreen.GetComponent<TextMeshPro>().color = new Color32(15, 250, 190, 255);
            }
            if(uiCounter.allRightHit > uiCounter.allLeftHit){
                rightScreen.GetComponent<TextMeshPro>().color = new Color32(250, 204, 16, 255);
                leftScreen.GetComponent<TextMeshPro>().color = new Color32(15, 250, 190, 255);
            }
            if (uiCounter.allRightHit == uiCounter.allLeftHit) {
                leftScreen.GetComponent<TextMeshPro>().color = new Color32(15, 250, 190, 255);
                rightScreen.GetComponent<TextMeshPro>().color = new Color32(15, 250, 190, 255);
            }

           
            //GUI.Button(new Rect(200,10, 130, 30),"Current Left Hit: " + leftHit);
            //GUI.Button(new Rect(200,40, 130, 30),"All Left Hits: " + uiCounter.allLeftHit);
            //GUI.Button(new Rect(500,10, 130, 30),"Current Right Hit: " + rightHit);
            //GUI.Button(new Rect(500,40, 130, 30),"All Right Hits: " +uiCounter.allRightHit);
            
            
            if (GUI.Button(new Rect(10, 60, 110, 40), "Stop + save EA"))
            {
                _neatSupervisor.StopEvolution();
                isStart = false;
            }
            /*
            if (GUI.Button(new Rect(10, 110, 110, 40), "Run best"))
            {
                _neatSupervisor.RunBest();
            }
            if (GUI.Button(new Rect(10, 160, 110, 40), "Delete Saves"))
            {
                ExperimentIO.DeleteAllSaveFiles(_neatSupervisor.Experiment);
            }
            */
            GUI.Label(new Rect(150, Screen.height - 425, 110, 60), "1 - 4 Change Cameras");
            GUI.Button(new Rect(10, Screen.height - 70, 110, 60), string.Format("Generation: {0}\nFitness: {1:0.0000}", _neatSupervisor.CurrentGeneration, _neatSupervisor.CurrentBestFitness));
            GUI.Button(new Rect(200, Screen.height - 70, 200, 60), string.Format("Left Fitness: {0:0.0000}\nRight Fitness: {1:0.0000}", leftFit, rightFit ));
            currentGen = _neatSupervisor.CurrentGeneration;

/*
            Grapher.Log(leftFit, "1_Left Fitness",currentGen);
            Grapher.Log(rightFit, "1_Right Fitness",currentGen);
            Grapher.Log(otherfencer1.GetComponent<FencerAIController>().currentLeftFencerStaminaBase, "2_Left Stamina",currentGen);
            Grapher.Log(fencer1.GetComponent<FencerAIController>().currentRightFencerStaminaBase, "2_Right Stamina",currentGen);
            Grapher.Log(uiCounter.allLeftHit, "3_All Left Hits",currentGen);
            Grapher.Log(uiCounter.allRightHit, "3_All Right Hits",currentGen);
            Grapher.Log(currentLeftAttemptedHits, "4_Attempted Left Hits",currentGen);
            Grapher.Log(currentRightAttemptedHits, "4_Attempted Right Hits",currentGen);
            Grapher.Log(uiCounter.allLeftDodges, "5_All Left Dodges",currentGen);
            Grapher.Log(uiCounter.allRightDodges, "5_All Right Dodges",currentGen);
            Grapher.Log(currentLeftDodges, "6_Attempted Left Dodge",currentGen);
            Grapher.Log(currentRightDodges, "6_Attempted Right Dodge",currentGen);
            Grapher.Log(uiCounter.allLeftZoneLeftFencer, "7_All Left Zone Left Fencer",currentGen);
            Grapher.Log(uiCounter.allLeftZoneRightFencer, "7_All Left Zone Right Fencer",currentGen);
            Grapher.Log(uiCounter.allRightZoneLeftFencer, "7_All Right Zone Left Fencer",currentGen);
            Grapher.Log(uiCounter.allRightZoneRightFencer, "7_All Right Zone Right Fencer",currentGen);
            */
            
        }
    }
}
