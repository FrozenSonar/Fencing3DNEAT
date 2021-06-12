
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
        public float Speed = 5f;
        public float TurnSpeed = 180f;
        public float SensorRange = 10;

    // track progress
        public int Lap = 1;
        public int CurrentPiece = 0;
        public int LastPiece = 0;
        public int WallHits = 0;

        private bool _movingForward = true;


        // cache the initial transform of this unit, to reset it on deactivation
        private Vector3 _initialPosition = new Vector3(10,1,0); // spawn position
        private Quaternion _initialRotation = default;

     private void Start()
        {
            // cache the inital transform of this Unit, so that when the Unit gets reset, it gets put into its initial state
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            transform.localScale = new Vector3(1,1,1);
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
            float frontSensor = 0;
            float leftFrontSensor = 0;
            float leftSensor = 0;
            float rightFrontSensor = 0;
            float rightSensor = 0;

            // Five raycasts into different directions each measure how far a wall is away.
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Fencer"))
                {
                    frontSensor = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0.5f, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Fencer"))
                {
                    rightFrontSensor = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(1, 0, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Fencer"))
                {
                    rightSensor = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-0.5f, 0, 1).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Fencer"))
                {
                    leftFrontSensor = 1 - hit.distance / SensorRange;
                }
            }

            if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-1, 0, 0).normalized), out hit, SensorRange))
            {
                if (hit.collider.CompareTag("Fencer"))
                {
                    leftSensor = 1 - hit.distance / SensorRange;
                }
            }

            // modify the ISignalArray object of the blackbox that was passed into this function, by filling it with the sensor information.
            // Make sure that NeatSupervisor.NetworkInputCount fits the amount of sensors you have
            inputSignalArray[0] = frontSensor;
            inputSignalArray[1] = leftFrontSensor;
            inputSignalArray[2] = leftSensor;
            inputSignalArray[3] = rightFrontSensor;
            inputSignalArray[4] = rightSensor;
        
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

            var steer = (float)outputSignalArray[0] * 2 - 1;
            var gas = (float)outputSignalArray[1] * 2 - 1;

            var moveDist = gas * Speed * Time.deltaTime;
            var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

            transform.Rotate(new Vector3(0, turnAngle, 0));
            transform.Translate(Vector3.forward * moveDist);
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