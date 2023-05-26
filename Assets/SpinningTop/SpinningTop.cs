using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpinningTop : MonoBehaviour
{
    private Rigidbody rb;
    public float angularVelocity;
    public float speed;
    public float gravityMultiplier;
    
    public Transform cam;
    private float horizontalInput;
    private float verticalInput;

    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = new Vector3(1,angularVelocity,0);
        rb.maxAngularVelocity = angularVelocity;
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;
        camF.y = 0;
        camR.y = 0;

        rb.AddForce(camF * (verticalInput * speed) + camR * (horizontalInput * speed));

        if (isGrounded())
        {
            rb.AddForce(Vector3.down * speed);
        }
        
    }

    bool isGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, ground);

        if(isGrounded)
        {
            return true;
        }
        else { return false; }
    }
}
