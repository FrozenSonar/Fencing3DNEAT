using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SabreHit : MonoBehaviour
{
   

    GameObject leftTarget;
    changeMaterial changeLeftColor;
    GameObject rightTarget;
    changeMaterial changeRightColor;

    hitCounter uiCounter;

    GameObject rightHand;

    public float sabreSensor;
    public float SabreRange = 15;

    public int currentLeftHit = 0;
    public int currentRightHit = 0;

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

        if (col.gameObject.tag == "Fencer") // Left side
                            {
                                if (currentLeftHit == 0 && currentRightHit == 0) {
                                    currentLeftHit = 1;
                                    uiCounter.allLeftHit += 1;
                                }
                                     
                            }
                if (col.gameObject.tag == "Other Fencer") // Right Side
                            {
                                if (currentLeftHit == 0 && currentRightHit == 0) {
                                    currentRightHit = 1;
                                    uiCounter.allRightHit += 1;
                                }
                                
                             
                            }

        
    }

     private void OnCollisionStay(Collision col) {

                if (col.gameObject.tag == "Fencer") // Left side
                            {
                                //If the GameObject has the same tag as specified, output this message in the console
                                //changeLeftColor.colortoGreen();
                                
                                //print(transform.root.tag);
                                //print("I've stabbed a Fencer!!");
                           
                                //print(currentLeftHit +"\n" + allLeftHit);
                            }
                if (col.gameObject.tag == "Other Fencer") // Right Side
                            {
                                //If the GameObject has the same tag as specified, output this message in the console
                               // changeRightColor.colortoGreen();
                                
                                //print(transform.root.tag);
                                //print("I've stabbed a Fencer!!");
                              
                               // print(currentRightHit +"\n" + allRightHit);
                            }


            }
            

    void OnCollisionExit(Collision col)
    {
         //changeLeftColor.colortoBlue();
         //changeRightColor.colortoBlue();
    }
    void Start()
    {
        uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();

        leftTarget = GameObject.Find("LeftTarget");
        changeLeftColor = leftTarget.GetComponent<changeMaterial>();

        rightTarget = GameObject.Find("RightTarget");
        changeRightColor = rightTarget.GetComponent<changeMaterial>();
    }
    // Update is called once per frame
    
}
