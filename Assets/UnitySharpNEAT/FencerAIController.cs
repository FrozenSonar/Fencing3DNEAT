
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySharpNEAT;
using SharpNeat.Phenomes;
/// <summary>
/// This class serves as script to create a UnitController.
/// </summary>

public class FencerAIController : UnitController
{
#region // Declare Variables
        // general control variables
        //public float Speed = 5f;
        public float TurnSpeed = 180f;
        public float SensorRange = 10;
        
        //Fencer Controller Variables
        public CharacterController characterController;
        public float speed;
        public Animator animator;
        public Animation anim;

        //sensors
        public float headSensor;
        public float legsSensor;
        public float chestSensor;
        public float bladeSensor;
        public float sphereSensor;

        // gravity
        private float gravity = 59.87f;
        private float verticalSpeed = 0;
        const float acceleration = 0.0067f;

        // track progress
        public int Lap = 1;
        public int CurrentPiece = 0;
        public int LastPiece = 0;
        public int WallHits = 0;

        // fencer stats to keep track
        public float currentLeftAttemptedHits = 0;
        public float currentRightAttemptedHits = 0;

        public float currentLeftDodges = 0;
        public float currentRightDodges = 0;

        public float currentLeftZoneLeftFencer = 0;
        public float currentLeftZoneRightFencer = 0;
        public float currentRightZoneLeftFencer = 0;
        public float currentRightZoneRightFencer = 0;

        private bool _movingForward = true;


        // cache the initial transform of this unit, to reset it on deactivation
        private Vector3 _initialPosition = new Vector3(10,0,0); // spawn position
        private Quaternion _initialRotation = default;

       
        private GameObject tobj;
        private GameObject fencer1;
        private GameObject otherfencer1;
        private GameObject sabreBlade;

        public SabreHit sabreHitScript;
        public hitCounter uiCounter;
        public NeatUI neatCounter;
        
        GameObject leftTarget;
        changeMaterial changeLeftColor;
        GameObject rightTarget;
        changeMaterial changeRightColor;
#endregion

     private void Start()
        {
            // cache the inital transform of this Unit, so that when the Unit gets reset, it gets put into its initial state
            //tobj = GameObject.Find("Target");
            sabreHitScript = GameObject.Find("Sword_blade").GetComponent<SabreHit>();
            fencer1 = GameObject.FindGameObjectsWithTag("Fencer")[0];
            otherfencer1 = GameObject.FindGameObjectsWithTag("Other Fencer")[0];
            neatCounter = GameObject.Find("NeatUI").GetComponent<NeatUI>();
            uiCounter = GameObject.Find("Cube").GetComponent<hitCounter>();
            leftTarget = GameObject.Find("LeftTarget");
            changeLeftColor = leftTarget.GetComponent<changeMaterial>();

            rightTarget = GameObject.Find("RightTarget");
            changeRightColor = rightTarget.GetComponent<changeMaterial>();
            
            if(transform.tag == "Fencer") {
                print("I am fencer " + transform.tag);
                sabreHitScript = fencer1.GetComponentInChildren<SabreHit>();
            }
            if(transform.tag == "Other Fencer") {
                print("I am other fencer " + transform.tag);
                sabreHitScript = otherfencer1.GetComponentInChildren<SabreHit>();
            }
            
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            transform.localScale = new Vector3(1,1,1);

        }
    
    private void Update() 
    {

        //Draw Rays on Fencer for Debugging 
        Debug.DrawRay(transform.position + transform.up + transform.up + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized) * SensorRange, Color.green); //Front Sensor Draw Ray
        Debug.DrawRay(transform.position + transform.up + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized) * SensorRange, Color.green); //Right Front Sensor Draw Ray
        Debug.DrawRay(transform.position + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized) * SensorRange, Color.green); //Left Front Sensor Draw Ray
        
        //print("Sabre Hits: " + sabreHitScript.timesHit);
        //Forcing Fencer to look at each other
            if(transform.tag == "Fencer")
            {
                Vector3 targetPosition = new Vector3( otherfencer1.transform.position.x, 
                                                        transform.position.y, 
                                                        otherfencer1.transform.position.z ) ;
                        transform.LookAt( targetPosition ) ;
            }
            
            if(transform.tag == "Other Fencer")
            {
                Vector3 targetPosition = new Vector3( fencer1.transform.position.x, 
                                                        transform.position.y, 
                                                        fencer1.transform.position.z ) ;
                        transform.LookAt( targetPosition ) ;
            }
            

    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward + transform.forward + transform.up + transform.up, 3);
    }

    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        // Called by the base class on FixedUpdate

        // Feed inputs into the Neural Net (IBlackBox) by modifying its InputSignalArray
        // The size of the input array corresponds to NeatSupervisor.NetworkInputCount

        /* EXAMPLE */
        //inputSignalArray[0] = someSensorValue;
        //inputSignalArray[1] = someOtherSensorValue;
        //...
            
            int layerMask = (1 << 6);
            // Five raycasts into different directions each measure how far a wall is away.
            RaycastHit hit;

            if (Physics.SphereCast(transform.position + transform.up + transform.up, 3, transform.forward + transform.forward, out hit, 3, layerMask))
            {
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    sphereSensor = 1 - hit.distance / 3;
                    //print("I've hit a: " + hit.collider.gameObject.name);
                    //print("Sphere Sensor: " + sphereSensor);
                }

                if (hit.collider.CompareTag("Blade"))
                {
                    bladeSensor = 1 - hit.distance / 3;
                    //print("I've hit a: " + hit.collider.gameObject.name);
                    //print("Blade Sensor: " + bladeSensor);
                }
            }

            //Debug.DrawRay(tranform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange), Color.green);
            if (Physics.Raycast(transform.position + transform.up + transform.up + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
               //print("I've not yet hit front!");
                
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    headSensor = 1 - hit.distance / SensorRange;
                    //print("Front Sensor: " + headSensor);
                    //print("Sabre Sensor: " + sabreHitScript.sabreSensor);
                   
                }
            }

            //if (Physics.Raycast(transform.position + transform.up + transform.up + transform.up * 1.1f, transform.TransformDirection(new Vector3(0.1f, 0, 1).normalized), out hit, SensorRange))
            if (Physics.Raycast(transform.position + transform.up + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    chestSensor = 1 - hit.distance / SensorRange;
                    //print("Right Front Sensor: " + chestSensor);
                }
            }

            //if (Physics.Raycast(transform.position + transform.up + transform.up + transform.up * 1.1f, transform.TransformDirection(new Vector3(-0.1f, 0, 1).normalized), out hit, SensorRange))
            if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    legsSensor = 1 - hit.distance / SensorRange;
                    //print("Left Front Sensor: " + legsSensor);
                }
            }

            // modify the ISignalArray object of the blackbox that was passed into this function, by filling it with the sensor information.
            // Make sure that NeatSupervisor.NetworkInputCount fits the amount of sensors you have
            inputSignalArray[0] = headSensor;
            inputSignalArray[1] = legsSensor;
            inputSignalArray[2] = chestSensor;
            inputSignalArray[3] = sphereSensor;
            inputSignalArray[4] = bladeSensor;
    }
    
    public float horizontalMove;
    public float verticalMove;
    public float attackRange;
    private IEnumerator coroutine;

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        // Called by the base class after the inputs have been processed
        // Read the outputs and do something with them
        // The size of the array corresponds to NeatSupervisor.NetworkOutputCount
        /* EXAMPLE */
        //someMoveDirection = outputSignalArray[0];
        //someMoveSpeed = outputSignalArray[1];
        //...

            animator.enabled = true;
            horizontalMove = (float)outputSignalArray[0];
            verticalMove = (float)outputSignalArray[1];
            speed = (float)outputSignalArray[2];
            attackRange = (float)outputSignalArray[3];

            var horizontalMove2 = (float)outputSignalArray[4];
            var verticalMove2 = (float)outputSignalArray[5];
            var speed2 = (float)outputSignalArray[6];
            var attackRange2 = (float)outputSignalArray[7];
        
            //print("I am attackRange: " + attackRange);

            //Gravity Working
            if (characterController.isGrounded) {
                verticalSpeed = 0;
            }
            else {
                verticalSpeed -= gravity * Time.deltaTime;
            }

            if(transform.tag == "Other Fencer") {
                horizontalMove = horizontalMove2;
                verticalMove = verticalMove2;
                speed = speed2;
                attackRange = attackRange2;
            }
           
            speed = Mathf.Clamp(speed, 0, 1);
            Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
            Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
            characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
            animator.SetFloat("Speed", speed);
            animator.SetBool("isWalking", verticalMove != 0 || horizontalMove != 0);
            animator.SetBool("isBackwards", verticalMove < 0 || horizontalMove < 0);
            
            coroutine = FencerRoutine();
            StartCoroutine(coroutine);

            

/*
                    uiCounter.allLeftZoneLeftFencer++;
                    uiCounter.allLeftZoneRightFencer++;
                    uiCounter.allRightZoneLeftFencer++;
                    uiCounter.allRightZoneRightFencer++;
                    currentLeftZoneLeftFencer++;
                    currentLeftZoneRightFencer++;
                    currentRightZoneLeftFencer++;
                    currentRightZoneRightFencer++; */
            

            //Stops Fencers when both are hit until new Generation
            if(isBothHit()) {
                //transform.position = _initialPosition;
                animator.SetFloat("Speed", 0);
                StopCoroutine(coroutine);
                animator.enabled = false;
                animator.Play("Idle", -1, 0f);
            }

            //Target at the back goes to green when either side hits
            if(neatCounter.leftHit == 1) {
                changeLeftColor.colortoGreen();
            }
            if(neatCounter.rightHit == 1) {
                changeRightColor.colortoGreen();
            }

            //Target resets color to blue at every generation
            if(neatCounter.leftHit == 0 && neatCounter.rightHit == 0){
                changeLeftColor.colortoBlue();
                changeRightColor.colortoBlue();
            }
    }

    public bool isBothHit(){
        if(neatCounter.leftHit == 1 || neatCounter.rightHit == 1){
            neatCounter.isBothHitUI = true;
            return true;
        }
        else {
            neatCounter.isBothHitUI = false;
            return false;
        }
    }

    IEnumerator FencerRoutine()
    {    
      
            if (headSensor >= 0.87) {
                animator.SetFloat("AttackSpeed", speed);
                StabCombo();
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attackmidslw") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack&Parry") || animator.GetCurrentAnimatorStateInfo(0).IsName("AttackJmpFwd")) 
                {
                    if (transform.tag == "Other Fencer" && !(isBothHit())){
                        currentLeftAttemptedHits++;
                        //print("Left Attempt Hits: " + currentLeftAttemptedHits);
                    }
                    if (transform.tag == "Fencer" && !(isBothHit())){
                        currentRightAttemptedHits++;
                        //print("Right Attempt Hits: " + currentRightAttemptedHits);
                    }

                }
                yield return null;
            }

        
            if (bladeSensor >= 0.985f) {
                animator.SetFloat("DodgeSpeed", attackRange);
                print("I'm dodging at "+ bladeSensor);
                Dodge();
                //if(animator.GetCurrentAnimatorStateInfo(0).IsName("ShortDodgeDwnwds") || animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeBwds") || animator.GetCurrentAnimatorStateInfo(0).IsName("ShortDodgeFast")) 
                //{
                    if (transform.tag == "Other Fencer" && !(isBothHit())){
                        currentLeftDodges++;
                        uiCounter.allLeftDodges++;
                        //print(currentLeftDodges);
                    }
                    if (transform.tag == "Fencer" && !(isBothHit())){
                        currentRightDodges++;
                        uiCounter.allRightDodges++;
                        //print(currentRightDodges);
                    }
                //}
                yield return null;
                
            }
            
            
            if (sphereSensor >= 0.6) {
                animator.SetFloat("SpecialSpeed", attackRange);
                Special();
                
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Move_fast_Rlow_1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Move_slow_Backtrick") || animator.GetCurrentAnimatorStateInfo(0).IsName("Sword1h_Taunt_mark_3") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Place_snap_Ldown_2") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Place_fast_Llow_1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Move_Achilles")) 
                {
                    if (transform.tag == "Other Fencer" && !(isBothHit())){
                        currentLeftAttemptedHits++;
                        //print("Left Attempt Hits: " + currentLeftAttemptedHits);
                    }
                    if (transform.tag == "Fencer" && !(isBothHit())){
                        currentRightAttemptedHits++;
                        //print("Right Attempt Hits: " + currentRightAttemptedHits);
                    }

                }

                yield return null;
            } 

    
            if (headSensor >= 0.6) {
                
                Stab();
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stabbing")) {

                    if (transform.tag == "Other Fencer" && !(isBothHit())){
                        currentLeftAttemptedHits++;
                        //print("Left Attempt Hits: " + currentLeftAttemptedHits);
                    }
                    if (transform.tag == "Fencer" && !(isBothHit())){
                        currentRightAttemptedHits++;
                        //print("Right Attempt Hits: " + currentRightAttemptedHits);
                    }

                    animator.speed = 2;
                }
                else {
                    animator.speed = 1;
                }
                yield return null;
            }
    }

    public void Stab()
    {
        animator.SetTrigger("goStab");
    }   

    public void StabCombo()
    {
        animator.SetTrigger("goStabCombo");
    }   

    public void Special()
    {
        animator.SetTrigger("goSpecial");
    }

     public void Dodge()
    {
        animator.SetTrigger("goDodge");
    }

    public void Idle()
    {
        animator.SetTrigger("goIdle");
    }

    public override float GetFitness()
    {
        // Called during the evaluation phase (at the end of each trail)
        // The performance of this unit, i.e. it's fitness, is retrieved by this function.
        // Implement a meaningful fitness function here
            /*
            if (Lap == 1 && CurrentPiece == 0)
            {
                return 0;
            }
            
            int piece = CurrentPiece;
            if (CurrentPiece == 0)
            {
                piece = 17;
            }

            float fit = Lap * piece - WallHits * 0.2f;
            if (fit > 0)
            {
                return fit;
            }
            return 0;
            */
            float leftAttempt = neatCounter.currentLeftAttemptedHits;
            float leftAllHits = uiCounter.allLeftHit;
            float rightAttempt = neatCounter.currentRightAttemptedHits;
            float rightAllHits = uiCounter.allRightHit;
            float leftDodges = neatCounter.currentLeftDodges;
            float rightDodges = neatCounter.currentRightDodges;
            float leftAllDodges = uiCounter.allLeftDodges;
            float rightAllDodges = uiCounter.allRightDodges;

            float allLeftDefendZone = uiCounter.allLeftZoneLeftFencer;  //Left Defend
            float allRightAttackZone = uiCounter.allLeftZoneRightFencer; //Right Attack
            float allLeftAttackZone = uiCounter.allRightZoneLeftFencer; // Left Attack
            float allRightDefendZone = uiCounter.allRightZoneRightFencer; // Right Defend

            float currentLeftDefendZone = neatCounter.currentLeftZoneLeftFencer; //Left Defend
            float currentRightAttackZone = neatCounter.currentLeftZoneRightFencer; //Right Attack
            float currentLeftAttackZone = neatCounter.currentRightZoneLeftFencer; // Left Attack
            float currentRightDefendZone = neatCounter.currentRightZoneRightFencer; // Right Defend

            if (leftAttempt == 0){
                leftAttempt = 1;
            }
            if (leftAllHits == 0){
                leftAllHits = 1;
            }

            if (rightAttempt == 0){
                rightAttempt = 1;
            }
            if (rightAllHits == 0){
                rightAllHits = 1;
            }

            if (leftAllDodges == 0){
                leftAllDodges = 1;
            }
            if (rightAllDodges == 0){
                rightAllDodges = 1;
            }

            if(allLeftAttackZone == 0){
                allLeftAttackZone = 1;
            }

            if(allRightAttackZone == 0){
                allRightAttackZone = 1;
            }

            //float fit = neatCounter.leftHit - neatCounter.rightHit + leftAllHits;
            //float fit = Mathf.Abs((neatCounter.leftHit + leftAllHits + (leftAttempt/leftAllHits)) - (neatCounter.rightHit + rightAllHits + (rightAttempt/rightAllHits)));
            float fit = 0;
            float growthRate = 0.57721f;
            float zoneLeftCalc = (currentLeftAttackZone/allLeftAttackZone) - (currentRightAttackZone/allRightAttackZone) + (currentLeftDefendZone/allLeftDefendZone);
            float zoneRightCalc = (currentRightAttackZone/allRightAttackZone) - (currentLeftAttackZone/allLeftAttackZone) + (currentRightDefendZone/allRightDefendZone);
            if (transform.tag == "Other Fencer") {
                //fit = Mathf.Abs((neatCounter.rightHit + rightAllHits + (rightAttempt/rightAllHits) + (rightDodges * 0.25f)) - (neatCounter.leftHit + leftAllHits + (leftAttempt/leftAllHits) + (leftDodges * 0.25f)));
                fit = Mathf.Abs(((neatCounter.leftHit - neatCounter.rightHit) + leftAllHits + (leftAttempt/leftAllHits) + ((leftAllHits - rightAllHits)) - ((rightDodges/rightAllDodges) + rightDodges) + (zoneLeftCalc)) * growthRate);
                neatCounter.leftFit = fit;
                print(zoneLeftCalc);
            }

            if (transform.tag == "Fencer") {
                //fit = Mathf.Abs((neatCounter.leftHit + leftAllHits + (leftAttempt/leftAllHits) + (leftDodges * 0.25f))  - (neatCounter.rightHit + rightAllHits + (rightAttempt/rightAllHits) + (rightDodges * 0.25f)));
                fit = Mathf.Abs(((neatCounter.rightHit - neatCounter.leftHit) + rightAllHits + (rightAttempt/rightAllHits) + ((rightAllHits - leftAllHits)) - ((leftDodges/leftAllDodges) + leftDodges) + (zoneRightCalc)) * growthRate);
                neatCounter.rightFit = fit;
                print(zoneRightCalc);
            }


            if (fit > 0)
            {
                return fit;
            }
            return 0;
       // return 0;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // Called whenever the value of IsActive has changed

        // Since NeatSupervisor.cs is making use of Object Pooling, this Unit will never get destroyed. 
        // Make sure that when IsActive gets set to false, the variables and the Transform of this Unit are reset!
        // Consider to also disable MeshRenderers until IsActive turns true again.

        // the unit has been deactivated, IsActive was switched to false

                // reset transform
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;

                // reset members
                Lap = 1;
                CurrentPiece = 0;
                LastPiece = 0;
                WallHits = 0;
                sabreHitScript.currentLeftHit = 0;
                sabreHitScript.currentRightHit = 0;

                currentLeftAttemptedHits = 0;
                currentRightAttemptedHits = 0;
                currentLeftDodges = 0;
                currentRightDodges = 0;

                currentLeftZoneLeftFencer = 0;
                currentLeftZoneRightFencer = 0;
                currentRightZoneLeftFencer = 0;
                currentRightZoneRightFencer = 0;

                _movingForward = true;

            // hide/show children 
            // the children happen to be the car meshes => we hide this Unit when IsActive turns false and show it when it turns true
            
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(newIsActive);
            }
            
    }
   
    private void OnTriggerStay(Collider col) {
        
        if(!(isBothHit())) {

                if (transform.tag == "Other Fencer")
                {
                        if(col.gameObject.name == "leftZone"){
                            uiCounter.allLeftZoneLeftFencer++;
                            currentLeftZoneLeftFencer++;
                        }

                        if(col.gameObject.name == "rightZone"){
                            uiCounter.allRightZoneLeftFencer++;
                            currentRightZoneLeftFencer++;
                        }

                }
                    if (transform.tag == "Fencer")
                {
                    if(col.gameObject.name == "leftZone"){
                            uiCounter.allLeftZoneRightFencer++;
                            currentLeftZoneRightFencer++;
                        }

                        if(col.gameObject.name == "rightZone"){
                            uiCounter.allRightZoneRightFencer++;
                            currentRightZoneRightFencer++;
                        }
                }
        }
    }
}