using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMultiTarget : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    private Vector3 velocity;

    public float minZoom = 10.0f;
    public float maxZoom = 40.0f;
    public float zoomLimiter = 50f;

    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(targets.Count == 0)
        {
            return;
        }
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i=0; i<targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
