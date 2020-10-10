using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraController : MonoBehaviour
{
    public float height;
    public float shakingDuration;
    public Transform cameraHolder;

    void Start()
    {
        cameraHolder.transform.position = new Vector3(transform.position.x, transform.position.y, height);
    }

    void Update()
    {
        cameraHolder.transform.position = new Vector3(transform.position.x, transform.position.y, height);
    }

    public void CameraShake()
    {
        CameraShaker.Instance?.ShakeOnce(4f, 4f, 0.1f, shakingDuration);
    }
}
