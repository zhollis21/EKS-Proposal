using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public MeshFilter OilMeshFilter;
    public MeshFilter WaterMeshFilter;
    public List<Transform> UpperRods;
    public Transform OilWaterLine;
    public Transform UpperFrontRight;
    public Transform LowerFrontRight;
    public float WaterTop;
    public float OilBottom;

    public enum Position { Left, Middle, Right }

    private List<Mesh> _oilMeshes = new List<Mesh>();
    private List<Mesh> _waterMeshes = new List<Mesh>();
    private float _xSlope;
    private float _xIntercept;
    private float _zSlope;
    private float _zIntercept;

    // Start is called before the first frame update
    void Start()
    {
        _oilMeshes.Add(OilMeshFilter?.mesh);
        _waterMeshes.Add(WaterMeshFilter?.mesh);

        // for each rod after the first one, we loop through add a copy of the original mesh with the same parent
        for (int i = 1; i < UpperRods.Count; i++)
        {
            _oilMeshes.Add(Instantiate(OilMeshFilter, OilMeshFilter?.transform?.parent)?.mesh);

            _waterMeshes.Add(Instantiate(WaterMeshFilter, WaterMeshFilter?.transform?.parent)?.mesh);
        }

        _xSlope = CalculateXOverYSlope(UpperFrontRight.position, LowerFrontRight.position);
        _xIntercept = CalculateXIntercept(UpperFrontRight.position, _xSlope);

        _zSlope = CalculateZOverYSlope(UpperFrontRight.position, LowerFrontRight.position);
        _zIntercept = CalculateZIntercept(UpperFrontRight.position, _zSlope);

        UpdateOilAndWaterMeshes();
    }

    // Update is called once per frame
    void Update()
    {
        OilWaterLine.position += (Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime) / 2;

        UpdateOilAndWaterMeshes();
    }

    private void UpdateOilAndWaterMeshes()
    {
        float lastRodYPosition = 0;

        for (int i = 0; i < _oilMeshes.Count; i++)
        {
            Position meshPosition;
            float xLeftMidPoint = 0;
            float xRightMidPoint = 0;

            if (i == 0)
            {
                meshPosition = Position.Left;

                float xRod = UpperRods[i].transform.position.x;
                float xRightRod = UpperRods[i + 1].transform.position.x;

                xRightMidPoint = (xRod + xRightRod) / 2;
            }
            else if (i == _oilMeshes.Count - 1)
            {
                meshPosition = Position.Right;

                float xRod = UpperRods[i].transform.position.x;
                float xLeftRod = UpperRods[i - 1].transform.position.x;

                xLeftMidPoint = (xRod + xLeftRod) / 2;
            }
            else
            {
                meshPosition = Position.Middle;

                float xRod = UpperRods[i].transform.position.x;
                float xLeftRod = UpperRods[i - 1].transform.position.x;
                float xRightRod = UpperRods[i + 1].transform.position.x;

                xRightMidPoint = (xRod + xRightRod) / 2;
                xLeftMidPoint = (xRod + xLeftRod) / 2;
            }

            float yTop = UpperRods[i].transform.position.y;

            _oilMeshes[i].vertices = GetVerticesForMeshAtYPoints(OilBottom, yTop, lastRodYPosition, xLeftMidPoint, xRightMidPoint, meshPosition);

            // Water should always have a flat top so we always assume WaterTop will also be the last y position
            _waterMeshes[i].vertices = GetVerticesForMeshAtYPoints(yTop, WaterTop, WaterTop, xLeftMidPoint, xRightMidPoint, meshPosition);

            lastRodYPosition = yTop;
        }
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

    private Vector3 GetFrontRightVerticesAtY(float y, float x, Position meshPosition)
    {
        Vector3 point = new Vector3(0, y, 0);

        // The right side of the mesh will always be straight vertical unless we are the far right mesh
        point.x = meshPosition == Position.Right ? _xSlope * y + _xIntercept : x;
        point.z = _zSlope * y + _zIntercept;

        return point;
    }

    private Vector3 GetFrontLeftVerticesAtY(float y, float x, Position meshPosition)
    {
        Vector3 point = new Vector3(0, y, 0);

        // The left side of the mesh will always be straight vertical unless we are the far left mesh
        point.x = meshPosition == Position.Left ? -(_xSlope * y + _xIntercept) : x;
        point.z = _zSlope * y + _zIntercept;

        return point;
    }

    private Vector3 GetBackRightVerticesAtY(float y, float x, Position meshPosition)
    {
        Vector3 point = new Vector3(0, y, 0);

        // The right side of the mesh will always be straight vertical unless we are the far right mesh
        point.x = meshPosition == Position.Right ? _xSlope * y + _xIntercept : x;
        point.z = -(_zSlope * y + _zIntercept);

        return point;
    }

    private Vector3 GetBackLeftVerticesAtY(float y, float x, Position meshPosition)
    {
        Vector3 point = new Vector3(0, y, 0);

        // The left side of the mesh will always be straight vertical unless we are the far left mesh
        point.x = meshPosition == Position.Left ? -(_xSlope * y + _xIntercept) : x;
        point.z = -(_zSlope * y + _zIntercept);

        return point;
    }

    private Vector3[] GetVerticesForMeshAtYPoints(float yBottom, float yTop, float previousRodY, float xLeftMidPoint, float xRightMidPoint, Position meshPosition)
    {
        var LowerFrontRight = GetFrontRightVerticesAtY(yBottom, xRightMidPoint, meshPosition);
        var LowerFrontLeft = GetFrontLeftVerticesAtY(yBottom, xLeftMidPoint, meshPosition);
        var LowerBackRight = GetBackRightVerticesAtY(yBottom, xRightMidPoint, meshPosition);
        var LowerBackLeft = GetBackLeftVerticesAtY(yBottom, xLeftMidPoint, meshPosition);
        var UpperFrontRight = GetFrontRightVerticesAtY(yTop, xRightMidPoint, meshPosition);
        var UpperFrontLeft = GetFrontLeftVerticesAtY(yTop, xLeftMidPoint, meshPosition);
        var UpperBackRight = GetBackRightVerticesAtY(yTop, xRightMidPoint, meshPosition);
        var UpperBackLeft = GetBackLeftVerticesAtY(yTop, xLeftMidPoint, meshPosition);

        // These are used to draw the left face to the height of the last rod
        var leftFaceLowerFrontLeft = GetFrontLeftVerticesAtY(previousRodY, xLeftMidPoint, meshPosition);
        var leftFaceUpperFrontLeft = GetFrontLeftVerticesAtY(yTop, xLeftMidPoint, meshPosition);
        var leftFaceUpperBackLeft = GetBackLeftVerticesAtY(yTop, xLeftMidPoint, meshPosition);
        var leftFaceLowerBackLeft = GetBackLeftVerticesAtY(previousRodY, xLeftMidPoint, meshPosition);

        return new Vector3[]
        {
            // Front Face
            LowerFrontRight,
            LowerFrontLeft,
            UpperFrontRight,
            UpperFrontLeft,

            // Back Face
            UpperBackRight,
            UpperBackLeft,
            LowerBackRight,
            LowerBackLeft,

            // Top Face
            UpperFrontRight,
            UpperFrontLeft,
            UpperBackRight,
            UpperBackLeft,

            // Bottom Face
            LowerBackRight,
            LowerFrontRight,
            LowerFrontLeft,
            LowerBackLeft,

            // Left Face (unless we are at the left we use the last rod's y position as the bottom
            meshPosition == Position.Left ? LowerFrontLeft : leftFaceLowerFrontLeft,
            meshPosition == Position.Left ? UpperFrontLeft : leftFaceUpperFrontLeft,
            meshPosition == Position.Left ? UpperBackLeft :  leftFaceUpperBackLeft,
            meshPosition == Position.Left ? LowerBackLeft :  leftFaceLowerBackLeft,

            // Right Face (we only draw this if we are the far right mesh)
            meshPosition == Position.Right ? LowerBackRight : Vector3.zero,
            meshPosition == Position.Right ? UpperBackRight : Vector3.zero,
            meshPosition == Position.Right ? UpperFrontRight : Vector3.zero,
            meshPosition == Position.Right ? LowerFrontRight : Vector3.zero,
        };
    }
}