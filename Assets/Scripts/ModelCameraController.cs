using UnityEngine;

public class ModelCameraController : MonoBehaviour
{
    public float RotationSpeed = 75;

    private Vector3 _parentPosition;

    // Start is called before the first frame update
    void Start()
    {
        _parentPosition = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_parentPosition == null)
            return;

        if (Input.GetAxis("Horizontal") < -0.3)
        {
            transform.RotateAround(_parentPosition, Vector3.up, Time.deltaTime * RotationSpeed);
        }
        else if (Input.GetAxis("Horizontal") > 0.3)
        {
            transform.RotateAround(_parentPosition, Vector3.down, Time.deltaTime * RotationSpeed);
        }
    }
}