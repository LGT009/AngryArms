using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Vector2 moveInput = Vector2.zero;
    [SerializeField] float speed = 1f;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Rigidbody playerRB;
    
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }
    //use rigdbody velocity
    // Update is called once per frame
    void Update()
    {
        Vector3 localMovement = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMovement = transform.TransformDirection(localMovement).normalized;

        targetVelocity = worldMovement* speed;
        playerRB.velocity = Vector3.SmoothDamp(playerRB.velocity, targetVelocity, ref currentVelocity, smoothTime);
    }

    public void OnMove(InputAction.CallbackContext ctx){
            moveInput = ctx.ReadValue<Vector2>();
    }
}
