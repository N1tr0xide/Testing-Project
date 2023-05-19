using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public static float _throttle;
    public static float _brakes;
    public static float _steering;

    // Update is called once per frame
    void Update()
    {
        _brakes = Convert.ToSingle(Input.GetKey(KeyCode.Space));
        _throttle = Input.GetAxis("Vertical");
        _steering = Input.GetAxis("Horizontal");
    }
}
