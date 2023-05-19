using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;
    private GameObject _targetPos;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _targetPos = _player.transform.Find("cameraTargetPos").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target = _targetPos.transform.position;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
        transform.LookAt(_player.transform);
    }
}
