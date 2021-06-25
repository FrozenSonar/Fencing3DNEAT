using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;

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

    public float currentLeftHit = 0;
    public float currentRightHit = 0;

    public NeatUI neatCounter;
    RaycastHit hit;

    void Update()
    {
        //0.966f
        /*
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
            */
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision col)
    {

    /*
        if (col.gameObject.tag == "Fencer") // Left side
                                    {
                                        if (!neatCounter.isBothHitUI) {
                                            currentLeftHit = 1;
                                            uiCounter.allLeftHit++;
                                        }
                                            
                                    }

                        if (col.gameObject.tag == "Other Fencer") // Right Side
                                    {
                                        if (!neatCounter.isBothHitUI) {
                                            currentRightHit = 1;
                                            uiCounter.allRightHit++;
                                        }
                                        
                                    
                                    }
    
        */

        
    }

     private void OnCollisionStay(Collision col) {

               if (col.gameObject.tag == "Fencer") // Left side
                                    {
                                        if (!neatCounter.isBothHitUI) {
                                            currentLeftHit = 1;
                                            uiCounter.allLeftHit++;
                                        }
                                            
                                    }

                        if (col.gameObject.tag == "Other Fencer") // Right Side
                                    {
                                        if (!neatCounter.isBothHitUI) {
                                            currentRightHit = 1;
                                            uiCounter.allRightHit++;
                                        }
                                        
                                    
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

        
        neatCounter = GameObject.Find("NeatUI").GetComponent<NeatUI>();
    }
    // Update is called once per frame
    
}
