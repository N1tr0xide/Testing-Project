using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float rotationSpeed;
    //public Transform orientation;

    /*[Header("Animation")]
    public Animator Animator;*/
        
    float horizontalInput;
    float forwardInput;
    Vector3 moveDirection;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Animator.SetBool("IsWalking", false);
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Inputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");   
    }

    void Movement()
    {
        //Movement Direction Calculation
        moveDirection = new Vector3(horizontalInput, 0, forwardInput);

        //Ground Movement
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Acceleration);
        //Animator.SetBool("IsWalking", rb.velocity != Vector3.zero);

        //Look at movement direction
        if(moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
          
    }
}
