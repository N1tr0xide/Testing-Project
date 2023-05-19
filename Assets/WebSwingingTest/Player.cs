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
        
    float _horizontalInput;
    float _forwardInput;
    Vector3 _moveDirection;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //Animator.SetBool("IsWalking", false);
        _rb.velocity = Vector3.zero;
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
        _horizontalInput = Input.GetAxis("Horizontal");
        _forwardInput = Input.GetAxis("Vertical");   
    }

    void Movement()
    {
        //Movement Direction Calculation
        _moveDirection = new Vector3(_horizontalInput, 0, _forwardInput);

        //Ground Movement
        _rb.AddForce(_moveDirection.normalized * speed * 10f, ForceMode.Acceleration);
        //Animator.SetBool("IsWalking", rb.velocity != Vector3.zero);

        //Look at movement direction
        if(_moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
          
    }
}
