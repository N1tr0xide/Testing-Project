using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [Header("Wheel Colliders (0 must be left, 1 must be right)")]  //0 must be left and 1 right
    public List<WheelCollider> FrontWheelColliders;  
    public List<WheelCollider> RearWheelColliders;

    [Header("Wheel Transforms (0 must be left, 1 must be right)")]  //0 must be left and 1 right
    public List<Transform> FrontWheelTransforms;
    public List<Transform> RearWheelTransforms;

    [Header("Drivetype")]  //if both are true the car is AWD
    public bool FWD;
    public bool RWD;

    public float acceleration = 1000f;
    public float brakingForce = 600f;
    public float maxTurnAngle = 40f;

    private float currentAcceleration = 0f;
    private float currentBrakingForce = 0f;
    private float currentTurnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        PlayerInputs(Convert.ToSingle(Input.GetKey(KeyCode.Space)));
        Steering();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Braking();
        Aceleration();
        
        VisualWheelUpdate(FrontWheelColliders[0], FrontWheelTransforms[0]);
        VisualWheelUpdate(FrontWheelColliders[1], FrontWheelTransforms[1]);
        VisualWheelUpdate(RearWheelColliders[0], RearWheelTransforms[0]);
        VisualWheelUpdate(RearWheelColliders[1], RearWheelTransforms[1]);
    }

    /// <summary>
    /// changes current braking force based on player input
    /// </summary>
    /// <param name="brakeInput"></param>
    void PlayerInputs(float brakeInput)
    {
        //Braking Input
        if (brakeInput != 0)
        {
            currentBrakingForce = brakingForce * brakeInput;
        }
        else
        {
            currentBrakingForce = 0f;
        }
    }

    void VisualWheelUpdate(WheelCollider col, Transform trans)
    {
        //get Wheel Collider State
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //Set wheel transform state
        trans.position = position;
        trans.rotation = rotation;
    }

    /// <summary>
    /// Apply braking torque to all wheels
    /// </summary>
    /// <param name="Input"></param>
    void Braking()
    {
        //Apply Braking to all Wheels
        foreach (WheelCollider Wheel in FrontWheelColliders)
        {
            Wheel.brakeTorque = currentBrakingForce;
        }
        foreach (WheelCollider Wheel in RearWheelColliders)
        {
            Wheel.brakeTorque = currentBrakingForce;
        }

        //Wheel Brake Test
        Debug.Log($"Brake Torque Test//  Front Left: {FrontWheelColliders[0].brakeTorque}, Front Right: {FrontWheelColliders[1].brakeTorque};  Rear Left: {RearWheelColliders[0].brakeTorque}, Rear Right: {RearWheelColliders[1].brakeTorque}");
    }

    void Aceleration()
    {
        //Input
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        //Apply acceleration to wheels
        if (FWD && RWD)
        {
            foreach (WheelCollider Wheel in FrontWheelColliders)
            {
                Wheel.motorTorque = currentAcceleration / 2;
            }

            foreach (WheelCollider Wheel in RearWheelColliders)
            {
                Wheel.motorTorque = currentAcceleration / 2;
            }
        }
        else if (RWD)
        {
            foreach (WheelCollider Wheel in RearWheelColliders)
            {
                Wheel.motorTorque = currentAcceleration;
            }
        }
        else if (FWD)
        {
            foreach (WheelCollider Wheel in FrontWheelColliders)
            {
                Wheel.motorTorque = currentAcceleration;
            }
        }

        /*Wheel Torque Test
        Debug.Log($"Wheel Torque Test//  Front Left: {FrontWheelColliders[0].motorTorque}, Front Right: {FrontWheelColliders[1].motorTorque}; Rear Left: {RearWheelColliders[0].motorTorque}, Rear Right: {RearWheelColliders[1].motorTorque}");*/
    }

    void Steering()
    {
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

        //Apply Steering
        foreach (WheelCollider Wheel in FrontWheelColliders)
        {
            Wheel.steerAngle = currentTurnAngle;
        }

        /*Wheel Steering Test
        Debug.Log($"Wheel Steering Test//  Front Left: {FrontWheelColliders[0].steerAngle}, Front Right: {FrontWheelColliders[1].steerAngle}");*/
    }
}