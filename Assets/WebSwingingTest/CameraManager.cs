using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public GameObject target;
    [FormerlySerializedAs("Sensitivity")] public float sensitivity = 4.0f;
    private float _targetDistance;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float _rotX;

    void Start()
    {
        _targetDistance = Vector3.Distance(transform.position, target.transform.position);
    }
    void Update()
    {
        //Mouse Inputs
        float y = Input.GetAxis("Mouse X") * sensitivity;
        _rotX += Input.GetAxis("Mouse Y") * sensitivity;

        // clamp the vertical rotation
        _rotX = Mathf.Clamp(_rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        transform.eulerAngles = new Vector3(-_rotX, transform.eulerAngles.y + y, 0);

        // move the camera position
        transform.position = target.transform.position - (transform.forward * _targetDistance);
    }
}
