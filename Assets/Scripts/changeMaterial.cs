using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMaterial : MonoBehaviour
{

    [SerializeField] private Material myMaterial;

    public void colortoGreen(){
        //myMaterial.color = Color.green;
        gameObject.GetComponent<Renderer> ().material.color = Color.green;
    }

    public void colortoBlue(){
        //myMaterial.color = Color.blue;
        gameObject.GetComponent<Renderer> ().material.color = Color.blue;
    }


    private void OnCollisionEnter(Collision col) {
         //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (col.gameObject.name == "Sabre")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            //colortoGreen();

        }
    }

    private void OnCollisionStay(Collision col) {
         //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (col.gameObject.name == "Sabre")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            colortoGreen();

        }
    }

    private void OnCollisionExit(Collision col) {
         //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (col.gameObject.name == "Sabre")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            colortoBlue();

        }
    }

}
