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
    public Transform UpperFrontLeft;
    public Transform UpperBackRight;
    public Transform UpperBackLeft;
    public Transform LowerFrontRight;
    public Transform LowerFrontLeft;
    public Transform LowerBackRight;
    public Transform LowerBackLeft;

    private Mesh _oilMesh;
    private Mesh _waterMesh;
    private float _xFrontRightSlope;
    private float _xFrontRightIntercept;
    private float _xFrontLeftSlope;
    private float _xFrontLeftIntercept;
    private float _xBackRightSlope;
    private float _xBackRightIntercept;
    private float _xBackLeftSlope;
    private float _xBackLeftIntercept;
    private float _zFrontRightSlope;
    private float _zFrontRightIntercept;
    private float _zFrontLeftSlope;
    private float _zFrontLeftIntercept;
    private float _zBackRightSlope;
    private float _zBackRightIntercept;
    private float _zBackLeftSlope;
    private float _zBackLeftIntercept;

    // Start is called before the first frame update
    void Start()
    {
        _oilMesh = OilMeshFilter?.mesh;
        _waterMesh = WaterMeshFilter?.mesh;

        CalculateSlopesAndIntercepts();

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

    // The below equations are based on an inverted form of Slope-Intercept
    private void CalculateSlopesAndIntercepts()
    {
        _xFrontRightSlope = (UpperFrontRight.position.x - LowerFrontRight.position.x) / (UpperFrontRight.position.y - LowerFrontRight.position.y);
        _xFrontRightIntercept = UpperFrontRight.position.x - _xFrontRightSlope * UpperFrontRight.position.y;

        _xFrontLeftSlope = (UpperFrontLeft.position.x - LowerFrontLeft.position.x) / (UpperFrontLeft.position.y - LowerFrontLeft.position.y);
        _xFrontLeftIntercept = UpperFrontLeft.position.x - _xFrontLeftSlope * UpperFrontLeft.position.y;

        _xBackRightSlope = (UpperBackRight.position.x - LowerBackRight.position.x) / (UpperBackRight.position.y - LowerBackRight.position.y);
        _xBackRightIntercept = UpperBackRight.position.x - _xBackRightSlope * UpperBackRight.position.y;

        _xBackLeftSlope = (UpperBackLeft.position.x - LowerBackLeft.position.x) / (UpperBackLeft.position.y - LowerBackLeft.position.y);
        _xBackLeftIntercept = UpperBackLeft.position.x - _xBackLeftSlope * UpperBackLeft.position.y;

        _zFrontRightSlope = (UpperFrontRight.position.z - LowerFrontRight.position.z) / (UpperFrontRight.position.y - LowerFrontRight.position.y);
        _zFrontRightIntercept = UpperFrontRight.position.z - _zFrontRightSlope * UpperFrontRight.position.y;

        _zFrontLeftSlope = (UpperFrontLeft.position.z - LowerFrontLeft.position.z) / (UpperFrontLeft.position.y - LowerFrontLeft.position.y);
        _zFrontLeftIntercept = UpperFrontLeft.position.z - _zFrontLeftSlope * UpperFrontLeft.position.y;

        _zBackRightSlope = (UpperBackRight.position.z - LowerBackRight.position.z) / (UpperBackRight.position.y - LowerBackRight.position.y);
        _zBackRightIntercept = UpperBackRight.position.z - _zBackRightSlope * UpperBackRight.position.y;

        _zBackLeftSlope = (UpperBackLeft.position.z - LowerBackLeft.position.z) / (UpperBackLeft.position.y - LowerBackLeft.position.y);
        _zBackLeftIntercept = UpperBackLeft.position.z - _zBackLeftSlope * UpperBackLeft.position.y;
    }

    private Vector3 GetFrontRightAtOilWaterLine()
    {
        var point = new Vector3(0, OilWaterLine.position.y, 0);

        point.x = _xFrontRightSlope * OilWaterLine.position.y + _xFrontRightIntercept;
        point.z = _zFrontRightSlope * OilWaterLine.position.y + _zFrontRightIntercept;

        return point;
    }

    private Vector3 GetFrontLeftAtOilWaterLine()
    {
        var point = new Vector3(0, OilWaterLine.position.y, 0);

        point.x = _xFrontLeftSlope * OilWaterLine.position.y + _xFrontLeftIntercept;
        point.z = _zFrontLeftSlope * OilWaterLine.position.y + _zFrontLeftIntercept;

        return point;
    }

    private Vector3 GetBackRightAtOilWaterLine()
    {
        var point = new Vector3(0, OilWaterLine.position.y, 0);

        point.x = _xBackRightSlope * OilWaterLine.position.y + _xBackRightIntercept;
        point.z = _zBackRightSlope * OilWaterLine.position.y + _zBackRightIntercept;

        return point;
    }

    private Vector3 GetBackLeftAtOilWaterLine()
    {
        var point = new Vector3(0, OilWaterLine.position.y, 0);

        point.x = _xBackLeftSlope * OilWaterLine.position.y + _xBackLeftIntercept;
        point.z = _zBackLeftSlope * OilWaterLine.position.y + _zBackLeftIntercept;

        return point;
    }

    private Vector3[] GetVerticesForWaterMesh()
    {
        // The Oil Water Line is the lower part of the water
        var WaterLowerFrontRight = GetFrontRightAtOilWaterLine();
        var WaterLowerFrontLeft = GetFrontLeftAtOilWaterLine();
        var WaterLowerBackRight = GetBackRightAtOilWaterLine();
        var WaterLowerBackLeft = GetBackLeftAtOilWaterLine();

        return new Vector3[]
        {
            // Front Face
            WaterLowerFrontRight,
            WaterLowerFrontLeft,
            UpperFrontRight.position,
            UpperFrontLeft.position,

            // Back Face
            UpperBackRight.position,
            UpperBackLeft.position,
            WaterLowerBackRight,
            WaterLowerBackLeft,

            // Top Face
            UpperFrontRight.position,
            UpperFrontLeft.position,
            UpperBackRight.position,
            UpperBackLeft.position,

            // Bottom Face
            WaterLowerBackRight,
            WaterLowerFrontRight,
            WaterLowerFrontLeft,
            WaterLowerBackLeft,

            // Left Face
            WaterLowerFrontLeft,
            UpperFrontLeft.position,
            UpperBackLeft.position,
            WaterLowerBackLeft,

            // Right Face
            WaterLowerBackRight,
            UpperBackRight.position,
            UpperFrontRight.position,
            WaterLowerFrontRight
        };
    }

    private Vector3[] GetVerticesForOilMesh()
    {
        // The Oil Water Line is the upper part of the oil 
        var OilUpperFrontRight = GetFrontRightAtOilWaterLine();
        var OilUpperFrontLeft = GetFrontLeftAtOilWaterLine();
        var OilUpperBackRight = GetBackRightAtOilWaterLine();
        var OilUpperBackLeft = GetBackLeftAtOilWaterLine();

        return new Vector3[]
        {
            // Front Face
            LowerFrontRight.position,
            LowerFrontLeft.position,
            OilUpperFrontRight,
            OilUpperFrontLeft,

            // Back Face
            OilUpperBackRight,
            OilUpperBackLeft,
            LowerBackRight.position,
            LowerBackLeft.position,

            // Top Face
            OilUpperFrontRight,
            OilUpperFrontLeft,
            OilUpperBackRight,
            OilUpperBackLeft,

            // Bottom Face
            LowerBackRight.position,
            LowerFrontRight.position,
            LowerFrontLeft.position,
            LowerBackLeft.position,

            // Left Face
            LowerFrontLeft.position,
            OilUpperFrontLeft,
            OilUpperBackLeft,
            LowerBackLeft.position,

            // Right Face
            LowerBackRight.position,
            OilUpperBackRight,
            OilUpperFrontRight,
            LowerFrontRight.position
        };
    }
}