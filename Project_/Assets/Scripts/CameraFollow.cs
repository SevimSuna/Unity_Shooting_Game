using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        //Kamera takibi
        Vector3 followPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, followPos, smoothSpeed);
        transform.position = smoothPos;

        transform.LookAt(target);
    }


}
