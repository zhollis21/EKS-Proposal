using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilController : MonoBehaviour
{
    public Transform UpperFrontRight;
    public Transform UpperFrontLeft;
    public Transform UpperBackRight;
    public Transform UpperBackLeft;
    public Transform LowerFrontRight;
    public Transform LowerFrontLeft;
    public Transform LowerBackRight;
    public Transform LowerBackLeft;

    private Mesh mf;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>().mesh;

        mf.vertices = GetCorrectVertices();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3[] GetCorrectVertices()
    {
        return new Vector3[]
        {
            LowerFrontRight.position,
            LowerFrontLeft.position,
            UpperFrontRight.position,
            UpperFrontLeft.position,
            UpperBackRight.position,
            UpperBackLeft.position,
            LowerBackRight.position,
            LowerBackLeft.position,
            UpperFrontRight.position,
            UpperFrontLeft.position,
            UpperBackRight.position,
            UpperBackLeft.position,
            LowerBackRight.position,
            LowerFrontRight.position,
            LowerFrontLeft.position,
            LowerBackLeft.position,
            LowerFrontLeft.position,
            UpperFrontLeft.position,
            UpperBackLeft.position,
            LowerBackLeft.position,
            LowerBackRight.position,
            UpperBackRight.position,
            UpperFrontRight.position,
            LowerFrontRight.position
        };
    }
}