using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;

    void Start(){
        cam1.tag = "MainCamera";
        cam2.tag = "Untagged";
        cam3.tag = "Untagged";
        cam4.tag = "Untagged";
        cam1.enabled = true;
        cam2.enabled = false;
        cam3.enabled = false;
        cam4.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("1Key")){
            cam1.tag = "MainCamera";
            cam2.tag = "Untagged";
            cam3.tag = "Untagged";
            cam4.tag = "Untagged";
            cam1.enabled = true;
            cam2.enabled = false;
            cam3.enabled = false;
            cam4.enabled = false;
        }
        if(Input.GetButtonDown("2Key")){
            cam1.tag = "Untagged";
            cam2.tag = "MainCamera";
            cam3.tag = "Untagged";
            cam4.tag = "Untagged";
            cam1.enabled = false;
            cam2.enabled = true;
            cam3.enabled = false;
            cam4.enabled = false;
        }
        if(Input.GetButtonDown("3Key")){
            cam1.tag = "Untagged";
            cam2.tag = "Untagged";
            cam3.tag = "MainCamera";
            cam4.tag = "Untagged";
            cam1.enabled = false;
            cam2.enabled = false;
            cam3.enabled = true;
            cam4.enabled = false;
        }
        if(Input.GetButtonDown("4Key")){
            cam1.tag = "Untagged";
            cam2.tag = "Untagged";
            cam3.tag = "Untagged";
            cam4.tag = "MainCamera";
            cam1.enabled = false;
            cam2.enabled = false;
            cam3.enabled = false;
            cam4.enabled = true;
        }
    }
}
