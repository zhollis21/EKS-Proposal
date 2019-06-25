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

        _oilMesh.vertices = GetVerticesForMeshAtYPoints(OilBottom, OilWaterLine.position.y);
        _waterMesh.vertices = GetVerticesForMeshAtYPoints(OilWaterLine.position.y, WaterTop);
    }

        // Update is called once per frame
        void Update()
    {
        OilWaterLine.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime;

        _oilMesh.vertices = GetVerticesForMeshAtYPoints(OilBottom, OilWaterLine.position.y);
        _waterMesh.vertices = GetVerticesForMeshAtYPoints(OilWaterLine.position.y, WaterTop);
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

    private Vector3[] GetVerticesForMeshAtYPoints(float yBottom, float yTop)
    {
        var LowerFrontRight = GetFrontRightVerticesAtY(yBottom);
        var LowerFrontLeft = GetFrontLeftVerticesAtY(yBottom);
        var LowerBackRight = GetBackRightVerticesAtY(yBottom);
        var LowerBackLeft = GetBackLeftVerticesAtY(yBottom);
        var UpperFrontRight = GetFrontRightVerticesAtY(yTop);
        var UpperFrontLeft = GetFrontLeftVerticesAtY(yTop);
        var UpperBackRight = GetBackRightVerticesAtY(yTop);
        var UpperBackLeft = GetBackLeftVerticesAtY(yTop);

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

            // Left Face
            LowerFrontLeft,
            UpperFrontLeft,
            UpperBackLeft,
            LowerBackLeft,

            // Right Face
            LowerBackRight,
            UpperBackRight,
            UpperFrontRight,
            LowerFrontRight
        };
    }
}