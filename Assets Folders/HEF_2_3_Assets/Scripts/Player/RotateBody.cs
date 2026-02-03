using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    [SerializeField] GameObject player;
    [SerializeField] float rotationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // player.transform.rotation = virtualCamera.transform.rotation;
        // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, virtualCamera.transform.eulerAngles.y, transform.eulerAngles.z);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, virtualCamera.transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
