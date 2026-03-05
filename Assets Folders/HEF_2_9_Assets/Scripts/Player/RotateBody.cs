using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    [SerializeField] Transform virtualCamera; // Use Transform instead of GameObject for direct access
    [SerializeField] float rotationSpeed = 10f; // Increased speed for snappier, smoother movement

    // Use LateUpdate for camera-related follow/rotation to avoid vibration
    void FixedUpdate()
    {
        if (virtualCamera == null) return;

        // 1. Get the target rotation (only Y axis if you want the body to stay upright)
        Quaternion targetRotation = Quaternion.Euler(0, virtualCamera.eulerAngles.y, 0);

        // 2. Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
