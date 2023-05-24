using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    public GameObject player;
    public Transform cam;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = new Vector3(0,1000,0);
        
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;
        camF.y = 0;
        camR.y = 0;

        Vector3 targetPos = transform.position - player.transform.position;
        
        rb.AddForce((camF - targetPos * speed) + (camR - targetPos * speed));
    }
}
