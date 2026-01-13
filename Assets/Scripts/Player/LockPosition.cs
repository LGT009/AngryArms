using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    public Transform parentTransform; // Assign the child GameObject's transform here

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            // Set the parent's position to match the child's position
            transform.position = parentTransform.position;
        }
    }
}