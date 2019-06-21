using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilController : MonoBehaviour
{
    public MeshFilter OilMeshFilter;
    public MeshFilter WaterMeshFilter;
    public Transform OilWaterLine;
    public Transform UpperFrontRight;
    public Transform LowerFrontRight;
    public float WaterTop;
    public float OilBottom;

    private Mesh _oilMesh;
    private Mesh _waterMesh;
    private float _xSlope;
    private float _xIntercept;
    private float _zSlope;
    private float _zIntercept;

    // Start is called before the first frame update
    void Start()
    {
        _oilMesh = OilMeshFilter?.mesh;
        _waterMesh = WaterMeshFilter?.mesh;

        _xSlope = CalculateXOverYSlope(UpperFrontRight.position, LowerFrontRight.position);
        _xIntercept = CalculateXIntercept(UpperFrontRight.position, _xSlope);

        _zSlope = CalculateZOverYSlope(UpperFrontRight.position, LowerFrontRight.position);
        _zIntercept = CalculateZIntercept(UpperFrontRight.position, _zSlope);

        _oilMesh.vertices = GetVerticesForOilMesh();
        _waterMesh.vertices = GetVerticesForWaterMesh();
    }

    // Update is called once per frame
    void Update()
    {
        OilWaterLine.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime;

        _oilMesh.vertices = GetVerticesForOilMesh();
        _waterMesh.vertices = GetVerticesForWaterMesh();
    }

    private float CalculateXOverYSlope(Vector3 P1, Vector3 P2)
    {
        return (P2.x - P1.x) / (P2.y - P1.y);
    }

    private float CalculateZOverYSlope(Vector3 P1, Vector3 P2)
    {
        return (P2.z - P1.z) / (P2.y - P1.y);
    }

    private float CalculateXIntercept(Vector3 P1, float xSlope)
    {
        return P1.x - xSlope * P1.y;
    }

    private float CalculateZIntercept(Vector3 P1, float zSlope)
    {
        return P1.z - zSlope * P1.y;
    }

    private Vector3 GetFrontRightVerticesAtY(float y)
    {
        var point = new Vector3(0, y, 0);

        point.x = _xSlope * y + _xIntercept;
        point.z = _zSlope * y + _zIntercept;

        return point;
    }

    private Vector3 GetFrontLeftVerticesAtY(float y)
    {
        var point = new Vector3(0, y, 0);

        point.x = -(_xSlope * y + _xIntercept);
        point.z = _zSlope * y + _zIntercept;

        return point;
    }

    private Vector3 GetBackRightVerticesAtY(float y)
    {
        var point = new Vector3(0, y, 0);

        point.x = _xSlope * y + _xIntercept;
        point.z = -(_zSlope * y + _zIntercept);

        return point;
    }

    private Vector3 GetBackLeftVerticesAtY(float y)
    {
        var point = new Vector3(0, y, 0);

        point.x = -(_xSlope * y + _xIntercept);
        point.z = -(_zSlope * y + _zIntercept);

        return point;
    }

    private Vector3[] GetVerticesForWaterMesh()
    {
        var WaterUpperFrontRight = GetFrontRightVerticesAtY(WaterTop);
        var WaterUpperFrontLeft = GetFrontLeftVerticesAtY(WaterTop);
        var WaterUpperBackRight = GetBackRightVerticesAtY(WaterTop);
        var WaterUpperBackLeft = GetBackLeftVerticesAtY(WaterTop);

        // The Oil Water Line is the lower part of the water
        var WaterLowerFrontRight = GetFrontRightVerticesAtY(OilWaterLine.position.y);
        var WaterLowerFrontLeft = GetFrontLeftVerticesAtY(OilWaterLine.position.y);
        var WaterLowerBackRight = GetBackRightVerticesAtY(OilWaterLine.position.y);
        var WaterLowerBackLeft = GetBackLeftVerticesAtY(OilWaterLine.position.y);

        return new Vector3[]
        {
            // Front Face
            WaterLowerFrontRight,
            WaterLowerFrontLeft,
            WaterUpperFrontRight,
            WaterUpperFrontLeft,

            // Back Face
            WaterUpperBackRight,
            WaterUpperBackLeft,
            WaterLowerBackRight,
            WaterLowerBackLeft,

            // Top Face
            WaterUpperFrontRight,
            WaterUpperFrontLeft,
            WaterUpperBackRight,
            WaterUpperBackLeft,

            // Bottom Face
            WaterLowerBackRight,
            WaterLowerFrontRight,
            WaterLowerFrontLeft,
            WaterLowerBackLeft,

            // Left Face
            WaterLowerFrontLeft,
            WaterUpperFrontLeft,
            WaterUpperBackLeft,
            WaterLowerBackLeft,

            // Right Face
            WaterLowerBackRight,
            WaterUpperBackRight,
            WaterUpperFrontRight,
            WaterLowerFrontRight
        };
    }

    private Vector3[] GetVerticesForOilMesh()
    {
        var OilLowerFrontRight = GetFrontRightVerticesAtY(OilBottom);
        var OilLowerFrontLeft = GetFrontLeftVerticesAtY(OilBottom);
        var OilLowerBackRight = GetBackRightVerticesAtY(OilBottom);
        var OilLowerBackLeft = GetBackLeftVerticesAtY(OilBottom);

        // The Oil Water Line is the upper part of the oil 
        var OilUpperFrontRight = GetFrontRightVerticesAtY(OilWaterLine.position.y);
        var OilUpperFrontLeft = GetFrontLeftVerticesAtY(OilWaterLine.position.y);
        var OilUpperBackRight = GetBackRightVerticesAtY(OilWaterLine.position.y);
        var OilUpperBackLeft = GetBackLeftVerticesAtY(OilWaterLine.position.y);

        return new Vector3[]
        {
            // Front Face
            OilLowerFrontRight,
            OilLowerFrontLeft,
            OilUpperFrontRight,
            OilUpperFrontLeft,

            // Back Face
            OilUpperBackRight,
            OilUpperBackLeft,
            OilLowerBackRight,
            OilLowerBackLeft,

            // Top Face
            OilUpperFrontRight,
            OilUpperFrontLeft,
            OilUpperBackRight,
            OilUpperBackLeft,

            // Bottom Face
            OilLowerBackRight,
            OilLowerFrontRight,
            OilLowerFrontLeft,
            OilLowerBackLeft,

            // Left Face
            OilLowerFrontLeft,
            OilUpperFrontLeft,
            OilUpperBackLeft,
            OilLowerBackLeft,

            // Right Face
            OilLowerBackRight,
            OilUpperBackRight,
            OilUpperFrontRight,
            OilLowerFrontRight
        };
    }
}