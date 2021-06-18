
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
        public float frontSensor;
        public float leftFrontSensor;
        public float rightFrontSensor;
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

        private bool _movingForward = true;


        // cache the initial transform of this unit, to reset it on deactivation
        private Vector3 _initialPosition = new Vector3(10,1,0); // spawn position
        private Quaternion _initialRotation = default;

       
        private GameObject tobj;
        private GameObject fencer1;
        private GameObject otherfencer1;
        private GameObject sabreBlade;
        public SabreHit sabreHitScript;
        

     private void Start()
        {
            // cache the inital transform of this Unit, so that when the Unit gets reset, it gets put into its initial state
            //tobj = GameObject.Find("Target");
            sabreHitScript = GameObject.Find("Sword_blade").GetComponent<SabreHit>();
            fencer1 = GameObject.FindGameObjectsWithTag("Fencer")[0];
            otherfencer1 = GameObject.FindGameObjectsWithTag("Other Fencer")[0];

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
    


    private void Update() {

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
                    frontSensor = 1 - hit.distance / SensorRange;
                    //print("Front Sensor: " + frontSensor);
                    //print("Sabre Sensor: " + sabreHitScript.sabreSensor);
                   
                }
                
                
            }

            
            //if (Physics.Raycast(transform.position + transform.up + transform.up + transform.up * 1.1f, transform.TransformDirection(new Vector3(0.1f, 0, 1).normalized), out hit, SensorRange))
            if (Physics.Raycast(transform.position + transform.up + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    rightFrontSensor = 1 - hit.distance / SensorRange;
                    //print("Right Front Sensor: " + rightFrontSensor);
                }

            }

            //if (Physics.Raycast(transform.position + transform.up + transform.up + transform.up * 1.1f, transform.TransformDirection(new Vector3(-0.1f, 0, 1).normalized), out hit, SensorRange))
            if (Physics.Raycast(transform.position + transform.up, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Other Fencer") || hit.collider.CompareTag("Fencer"))
                {
                    leftFrontSensor = 1 - hit.distance / SensorRange;
                    //print("Left Front Sensor: " + leftFrontSensor);
                }

            }



            // modify the ISignalArray object of the blackbox that was passed into this function, by filling it with the sensor information.
            // Make sure that NeatSupervisor.NetworkInputCount fits the amount of sensors you have
            inputSignalArray[0] = frontSensor;
            inputSignalArray[1] = leftFrontSensor;
            inputSignalArray[2] = rightFrontSensor;
            inputSignalArray[3] = sphereSensor;
            inputSignalArray[4] = bladeSensor;

        
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputSignalArray)
    {
        // Called by the base class after the inputs have been processed

        // Read the outputs and do something with them
        // The size of the array corresponds to NeatSupervisor.NetworkOutputCount


        /* EXAMPLE */
        //someMoveDirection = outputSignalArray[0];
        //someMoveSpeed = outputSignalArray[1];
        //...

            //var steer = (float)outputSignalArray[0] * 2 - 1;
            //var gas = (float)outputSignalArray[1] * 2 - 1;

            //var moveDist = gas * Speed * Time.deltaTime;
            //var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

            //transform.Rotate(new Vector3(0, turnAngle, 0));
            //transform.Translate(Vector3.forward * moveDist);

            var horizontalMove = (float)outputSignalArray[0];
            var verticalMove = (float)outputSignalArray[1];
            speed = (float)outputSignalArray[2];
            var attackRange = (float)outputSignalArray[3];

            var horizontalMove2 = (float)outputSignalArray[4];
            var verticalMove2 = (float)outputSignalArray[5];
            var speed2 = (float)outputSignalArray[6];
            var attackRange2 = (float)outputSignalArray[7];

            //print("I am attackRange: " + attackRange);
            //print("I am horizontalMove: "+ horizontalMove);
            //print("I am horizontalMove 2: "+ horizontalMove2);
            //print("Close Range: "+ closeRange);
            //print("I am speed: "+ speed);
            
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

             if (frontSensor >= 0.87) {
                animator.SetFloat("AttackSpeed", speed);
                StabCombo();
            }

            if (bladeSensor >= 0.87) {
                animator.SetFloat("DodgeSpeed", attackRange);
                Dodge();
            }

            if (sphereSensor <= 0.7) {
                 Flip();
            }

            if (frontSensor >= 0.6) {
                Stab();
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stabbing")) {
                    animator.speed = 2;
                }
                else {
                    animator.speed = 1;
                }
            }
            
            speed = Mathf.Clamp(speed, 0, 1);
            Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
        
            Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
            characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);

            animator.SetFloat("Speed", speed);
            animator.SetBool("isWalking", verticalMove != 0 || horizontalMove != 0);
            animator.SetBool("isBackwards", verticalMove < 0 || horizontalMove < 0);

    }

    public void Stab()
    {
        animator.SetTrigger("goStab");
    }   

    public void StabCombo()
    {
        animator.SetTrigger("goStabCombo");
    }   

    public void Flip()
    {
        animator.SetTrigger("goFlip");
    }

     public void Dodge()
    {
        animator.SetTrigger("goDodge");
    }


    public override float GetFitness()
    {
        // Called during the evaluation phase (at the end of each trail)

        // The performance of this unit, i.e. it's fitness, is retrieved by this function.
        // Implement a meaningful fitness function here
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
                _movingForward = true;

            // hide/show children 
            // the children happen to be the car meshes => we hide this Unit when IsActive turns false and show it when it turns true
            
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(newIsActive);
            }
            
    }
    public void NewLap()
        {
            if (LastPiece > 2 && _movingForward)
            {
                Lap++;
            }
        }

    private void OnCollisionEnter(Collision collision)
        {
            if (!IsActive)
                return;

            if (collision.collider.CompareTag("Road"))
            {
                RoadPiece rp = collision.collider.GetComponent<RoadPiece>();
                //  print(collision.collider.tag + " " + rp.PieceNumber);

                if ((rp.PieceNumber != LastPiece) && (rp.PieceNumber == CurrentPiece + 1 || (_movingForward && rp.PieceNumber == 0)))
                {
                    LastPiece = CurrentPiece;
                    CurrentPiece = rp.PieceNumber;
                    _movingForward = true;
                }
                else
                {
                    _movingForward = false;
                }
                if (rp.PieceNumber == 0)
                {
                    CurrentPiece = 0;
                }
            }
            else if (collision.collider.CompareTag("Wall"))
            {
                WallHits++;
            }
        }
    
}