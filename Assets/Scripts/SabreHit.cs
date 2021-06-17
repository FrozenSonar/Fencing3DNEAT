using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SabreHit : MonoBehaviour
{
    public int timesHit = 0;
    GameObject leftTarget;
    changeMaterial changeLeftColor;
    GameObject rightTarget;
    changeMaterial changeRightColor;

    GameObject rightHand;

    public float sabreSensor;
    public float SabreRange = 15;
    RaycastHit hit;

    void Update()
    {
        //0.966f
        Debug.DrawRay(transform.parent.position * 0.966f, transform.TransformDirection(new Vector3(0, 90, 0).normalized) * SabreRange, Color.red); //Front Sensor Draw Ray
        if (Physics.Raycast(transform.parent.position * 0.966f, transform.TransformDirection(new Vector3(0, 90, 0).normalized), out hit, SabreRange))
            {
               //print("I've not yet hit front!");
                
                if (hit.collider.CompareTag("Other Fencer"))
                {
                    sabreSensor = 1 - hit.distance / SabreRange;
                    //print("Sabre Sensor: " + sabreSensor);
                   
                }


            }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision col)
    {

        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (col.gameObject.name == "Target")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            //print("I'm stabbing a Target!");
            
            //changeLeftColor.colortoGreen();
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
                                changeLeftColor.colortoGreen();
                                
                                //print(transform.root.tag);
                                //print("I've stabbed a Fencer!!");
                                timesHit += 1;
                            }
                if (col.gameObject.tag == "Other Fencer")
                            {
                                //If the GameObject has the same tag as specified, output this message in the console
                                changeRightColor.colortoGreen();
                                
                                //print(transform.root.tag);
                                //print("I've stabbed a Fencer!!");
                                timesHit += 1;
                            }


            }
            

    void OnCollisionExit(Collision col)
    {
         changeLeftColor.colortoBlue();
         changeRightColor.colortoBlue();
    }
    void Start()
    {
        leftTarget = GameObject.Find("LeftTarget");
        changeLeftColor = leftTarget.GetComponent<changeMaterial>();

        rightTarget = GameObject.Find("RightTarget");
        changeRightColor = rightTarget.GetComponent<changeMaterial>();
    }
    // Update is called once per frame
    
}
