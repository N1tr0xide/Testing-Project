using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [Header("Wheel Colliders (0-left, 1-right)")]  //0 must be left and 1 right
    public WheelCollider[] frontWheelColliders;  
    public WheelCollider[] rearWheelColliders;

    [Header("Wheel Transforms (0-left, 1-right)")]  //0 must be left and 1 right
    public Transform[] frontWheelTransforms;
    public Transform[] rearWheelTransforms;

    private Rigidbody rb;
    public float kph;
    
    internal enum drivetype
    {
        fwd,
        rwd,
        awd
    }
    [SerializeField]private drivetype drive;
    
    public float torque = 1000f;
    public float brakingForce = 600f;
    private float _rearTrackSize;
    private float _wheelBase;
    public float radius;
    //public float maxTurnAngle = 40f;

    private float _currentTorque = 0f;
    private float _currentBrakingForce = 0f;
    //private float _currentTurnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _rearTrackSize = (rearWheelColliders[0].transform.position - rearWheelColliders[1].transform.position).magnitude;
        _wheelBase = (rearWheelColliders[0].transform.position - frontWheelColliders[0].transform.position).magnitude;
    }

    void Update()
    {
        PlayerInputs();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Braking();
        Aceleration();
        Steering();
        VisualWheelUpdate();
    }

    void PlayerInputs()
    {
        float brakeInput = Convert.ToSingle(Input.GetKey(KeyCode.Space));
        float throttleInput = Input.GetAxis("Vertical");

        //Braking Input
        _currentBrakingForce = brakingForce * brakeInput;
        
        //Throttle Input
        _currentTorque = torque * throttleInput;
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

        kph = rb.velocity.magnitude * 3.6f;

        /*Wheel Torque Test
        Debug.Log($"Wheel Torque Test//  Front Left: {frontWheelColliders[0].motorTorque}, Front Right: {frontWheelColliders[1].motorTorque}; Rear Left: {rearWheelColliders[0].motorTorque}, Rear Right: {rearWheelColliders[1].motorTorque}");*/
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Apply Steering to wheels
    /// </summary>
    void Steering()
    {
        float steeringInput = Input.GetAxis("Horizontal");

        //Ackerman Steering
        //steerAngle = Mathf.Rad2Deg * mathf.Atan(Wheel Base / (radius +- (Rear Tacks Size / 2))) * steeringInput;
        if (steeringInput > 0)
        {
            frontWheelColliders[0].steerAngle =
                Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (radius + (_rearTrackSize / 2))) * steeringInput;
            frontWheelColliders[1].steerAngle =
                Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (radius - (_rearTrackSize / 2))) * steeringInput;
        }
        else if (steeringInput < 0)
        {
            frontWheelColliders[0].steerAngle =
                Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (radius - (_rearTrackSize / 2))) * steeringInput;
            frontWheelColliders[1].steerAngle =
                Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (radius + (_rearTrackSize / 2))) * steeringInput;
        }
        else
        {
            frontWheelColliders[0].steerAngle = 0;
            frontWheelColliders[1].steerAngle = 0;
        }
        
        //Wheel Steering Test
        //Debug.Log($"Wheel Steering Test//  Front Left: {frontWheelColliders[0].steerAngle}, Front Right: {frontWheelColliders[1].steerAngle}");
        //Debug.Log(_reartrackSize);
        //Debug.Log(_wheelBase);
    }
}