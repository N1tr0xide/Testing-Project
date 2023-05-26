using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with " + collision.gameObject.name);

        Vector3 camF = cam.forward;
        Vector3 camR = cam.right;
        camF.y = 0;
        camR.y = 0;

        Vector3 targetPos = collision.transform.position - gameObject.transform.position;

        if (collision.gameObject.name == "Enemy")
        {
            collision.rigidbody.AddForce((camF + targetPos * speed) + (camR + targetPos * speed), ForceMode.Impulse);
            rb.AddForce((camF - targetPos * (speed / 4)) + (camR - targetPos * (speed / 4)), ForceMode.Impulse);
        }
    }
}
