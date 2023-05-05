using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target;
    public float Sensitivity = 4.0f;
    private float targetDistance;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float rotX;

    void Start()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
    }
    void Update()
    {
        //Mouse Inputs
        float y = Input.GetAxis("Mouse X") * Sensitivity;
        rotX += Input.GetAxis("Mouse Y") * Sensitivity;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);

        // move the camera position
        transform.position = target.transform.position - (transform.forward * targetDistance);
    }
}
