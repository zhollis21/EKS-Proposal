using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform rotateAroundObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") < -.3)
        {
            transform.RotateAround(rotateAroundObject.position, Vector3.up, Time.deltaTime * 100);
        }
        else if (Input.GetAxis("Horizontal") > .3)
        {
            transform.RotateAround(rotateAroundObject.position, Vector3.up, Time.deltaTime * 100);
        }
    }
}
