using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSabreHit : MonoBehaviour
{

    GameObject referenceObject;
    changeMaterial changeColor;
    GameObject rightHand;

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision col)
    {

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (col.gameObject.name == "Target")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            print("I'm stabbing a Target!");

        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (col.gameObject.tag == "Fencer")
        {
            //If the GameObject has the same tag as specified, output this message in the console
        }

    }

     private void OnCollisionStay(Collision col) {
                if (col.gameObject.tag == "Fencer")
                            {
                                //If the GameObject has the same tag as specified, output this message in the console
                                changeColor.colortoGreen();
                                
                                print(transform.root.tag);
                                print("I've stabbed a Fencer!!");
                            }
    

            }
            

    void OnCollisionExit(Collision col)
    {
         changeColor.colortoBlue();
    }
    void Start()
    {

        referenceObject = GameObject.Find("Target");
        changeColor = referenceObject.GetComponent<changeMaterial>();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
