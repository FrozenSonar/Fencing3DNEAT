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

namespace UnitySharpNEAT
{
    public class NeatUI : MonoBehaviour
    {
        [SerializeField] 
        private NeatSupervisor _neatSupervisor;

        /// <summary>
        /// Display simple Onscreen buttons for quickly accessing ceratain lifecycle funtions and to display generation info.
        /// </summary>


        public SabreHit sabreHitLeftScripts;
        public SabreHit sabreHitRightScripts;
        public int leftHit;
        public int rightHit;
        hitCounter uiCounter;
        private GameObject fencer1;
        private GameObject otherfencer1;
        
        private void Start(){
            uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();
            
        }

        private void OnGUI()
        {
            
            if (GUI.Button(new Rect(10, 10, 110, 40), "Start EA"))
            {
                _neatSupervisor.StartEvolution();
            }

            fencer1 = GameObject.FindGameObjectsWithTag("Fencer")[0];
            sabreHitRightScripts = fencer1.GetComponentInChildren<SabreHit>();
            otherfencer1 = GameObject.FindGameObjectsWithTag("Other Fencer")[0];
            
            sabreHitLeftScripts = otherfencer1.GetComponentInChildren<SabreHit>();
            leftHit = sabreHitLeftScripts.currentLeftHit;
            rightHit = sabreHitRightScripts.currentRightHit;
            //sabreHitScripts = GameObject.Find("Sword_blade").GetComponent<SabreHit>();
            
           
            GUI.Button(new Rect(200,10, 130, 30),"Current Left Hit: " + leftHit);
            GUI.Button(new Rect(200,40, 130, 30),"All Left Hits: " + uiCounter.allLeftHit);
            GUI.Button(new Rect(500,10, 130, 30),"Current Right Hit: " + rightHit);
            GUI.Button(new Rect(500,40, 130, 30),"All Right Hits: " +uiCounter.allRightHit);
            
            /*
            if (GUI.Button(new Rect(10, 60, 110, 40), "Stop + save EA"))
            {
                _neatSupervisor.StopEvolution();
            }
            if (GUI.Button(new Rect(10, 110, 110, 40), "Run best"))
            {
                _neatSupervisor.RunBest();
            }
            if (GUI.Button(new Rect(10, 160, 110, 40), "Delete Saves"))
            {
                ExperimentIO.DeleteAllSaveFiles(_neatSupervisor.Experiment);
            }
            */
            GUI.Button(new Rect(10, Screen.height - 70, 110, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", _neatSupervisor.CurrentGeneration, _neatSupervisor.CurrentBestFitness));
        }
    }
}
