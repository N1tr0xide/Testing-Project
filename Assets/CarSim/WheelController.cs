using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WheelController : MonoBehaviour
{
    [Header("Wheel Colliders (0-left, 1-right)")]  //0 must be left and 1 right
    public WheelCollider[] frontWheelColliders;  
    public WheelCollider[] rearWheelColliders;

    [Header("Wheel Transforms (0-left, 1-right)")]  //0 must be left and 1 right
    public Transform[] frontWheelTransforms;
    public Transform[] rearWheelTransforms;
    
    internal enum drivetype
    {
        fwd,
        rwd,
        awd
    }
    [SerializeField]private drivetype drive;
    
    public float torque = 1000f;
    public float brakingForce = 600f;
    public float maxTurnAngle = 40f;

    private float _currentTorque = 0f;
    private float _currentBrakingForce = 0f;
    private float _currentTurnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        PlayerInputs(inputManager._brakes, inputManager._throttle, inputManager._steering);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Braking();
        Aceleration();
        Steering();
        VisualWheelUpdate();
    }

    void PlayerInputs(float brakeInput, float throttleInput, float steeringInput)
    {
        //Braking Input
        _currentBrakingForce = brakingForce * brakeInput;
        
        //Throttle Input
        _currentTorque = torque * throttleInput;
        
        //Steering Input
        _currentTurnAngle = maxTurnAngle * steeringInput;
    }

    void VisualWheelUpdate()
    {
        //get Wheel Collider State
        Vector3 position;
        Quaternion rotation;
        
        for(int i = 0; i < frontWheelColliders.Length; i++)
        {
            frontWheelColliders[i].GetWorldPose(out position, out rotation);
            frontWheelTransforms[i].position = position;
            frontWheelTransforms[i].rotation = rotation;
        }
        
        for(int i = 0; i < rearWheelColliders.Length; i++)
        {
            rearWheelColliders[i].GetWorldPose(out position, out rotation);
            rearWheelTransforms[i].position = position;
            rearWheelTransforms[i].rotation = rotation;
        }
    }

    /// <summary>
    /// Apply braking torque to all wheels
    /// </summary>
    /// <param name="Input"></param>
    void Braking()
    {
        //Apply Braking to all Wheels
        foreach (WheelCollider wheel in frontWheelColliders)
        {
            wheel.brakeTorque = _currentBrakingForce;
        }
        foreach (WheelCollider wheel in rearWheelColliders)
        {
            wheel.brakeTorque = _currentBrakingForce;
        }

        /*Wheel Brake Test
        Debug.Log($"Brake Torque Test//  Front Left: {frontWheelColliders[0].brakeTorque}, Front Right: {frontWheelColliders[1].brakeTorque};  Rear Left: {rearWheelColliders[0].brakeTorque}, Rear Right: {rearWheelColliders[1].brakeTorque}");*/
    }

    /// <summary>
    /// Apply Motor torque to wheels depending on drivetype
    /// </summary>
    void Aceleration()
    {
        float totalTorque = _currentTorque;
        
        //Apply acceleration to wheels
        if (drive == drivetype.awd)
        {
            foreach (WheelCollider wheel in frontWheelColliders)
            {
                wheel.motorTorque = totalTorque / 4;
            }

            foreach (WheelCollider wheel in rearWheelColliders)
            {
                wheel.motorTorque = totalTorque / 4;
            }
        }
        else if (drive == drivetype.rwd)
        {
            foreach (WheelCollider wheel in rearWheelColliders)
            {
                wheel.motorTorque = totalTorque / 2;
            }
        }
        else if (drive == drivetype.fwd)
        {
            foreach (WheelCollider wheel in frontWheelColliders)
            {
                wheel.motorTorque = totalTorque / 2;
            }
        }

        /*Wheel Torque Test
        Debug.Log($"Wheel Torque Test//  Front Left: {frontWheelColliders[0].motorTorque}, Front Right: {frontWheelColliders[1].motorTorque}; Rear Left: {rearWheelColliders[0].motorTorque}, Rear Right: {rearWheelColliders[1].motorTorque}");*/
    }

    /// <summary>
    /// Apply Steering to wheels
    /// </summary>
    void Steering()
    {
        //Apply Steering
        foreach (WheelCollider wheel in frontWheelColliders)
        {
            wheel.steerAngle = _currentTurnAngle;
        }

        /*Wheel Steering Test
        Debug.Log($"Wheel Steering Test//  Front Left: {FrontWheelColliders[0].steerAngle}, Front Right: {FrontWheelColliders[1].steerAngle}");*/
    }
}