using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimPunch : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject leftFist;
    [SerializeField] GameObject rightFist;
    [SerializeField] GameObject fists;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    [SerializeField] float scale = 1f;

    private float prevMouseX = 0.0f;
    private float prevMouseY = 0.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;

    private Vector2 mouseDelta = Vector2.zero;

    private float currentEulerX = 0.0f;
    private float currentEulerY = 0.0f;

    void Start()
    {
        
        if (Mouse.current != null)
        {
            Mouse.current.WarpCursorPosition(Vector3.zero); 
        }
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseX = mouseScreenPosition.x;
        mouseY = mouseScreenPosition.y;

        // float xDifference = (mouseX - prevMouseX) / scale;
        // float yDifference = (mouseY - prevMouseY) / scale;

        if (mouseDelta.magnitude != 0)
        {
            currentEulerY += mouseDelta.x;
            currentEulerX -= mouseDelta.y; 

            currentEulerY = Mathf.Clamp(currentEulerY, minX, maxX);
            currentEulerX = Mathf.Clamp(currentEulerX, minY, maxY);

            //Debug.Log(playerTransform.rotation.y + minY);

            Quaternion targetRotation = Quaternion.Euler(currentEulerX, currentEulerY, 0);

            leftFist.transform.rotation = playerTransform.rotation * targetRotation;
            rightFist.transform.rotation = playerTransform.rotation * targetRotation; 

            prevMouseX = mouseX;
            prevMouseY = mouseY;
        }
    }

    public void OnMouseMove(InputAction.CallbackContext ctx){
        mouseDelta = ctx.ReadValue<Vector2>();
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class AimPunch : MonoBehaviour
// {
//     [SerializeField] GameObject leftFist;
//     [SerializeField] GameObject rightFist;
//     [SerializeField] GameObject fists;

//     [SerializeField] float minX;
//     [SerializeField] float maxX;

//     [SerializeField] float minY;
//     [SerializeField] float maxY;
    
//     private float prevMouseX = 0.0f;
//     private float prevMouseY = 0.0f;

//     private float mouseX = 0.0f;
//     private float mouseY = 0.0f;

//     [SerializeField] float scale = 1f;

//     // Start is called before the first frame update
//     void Start()
//     {
//         Mouse.current.WarpCursorPosition(Vector3.zero);
//         // Vector3 mouseScreenPosition = Input.mousePosition;
//         // prevMouseX = mouseScreenPosition.x;
//         // prevMouseY = mouseScreenPosition.y;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Vector3 mouseScreenPosition = Input.mousePosition;
//         mouseX = mouseScreenPosition.x;
//         mouseY = mouseScreenPosition.y;

//         //Debug.Log("X: " + mouseX + " Y: " + mouseY);
//         float xDifference = (mouseX - prevMouseX)/scale;
//         float yDifference = (mouseY - prevMouseY)/scale;

//         //Debug.Log("X: " + xDifference);
//         //Debug.Log("Y: " + yDifference);
    
//         if(xDifference != 0 || yDifference != 0){
//             Quaternion leftFistRotation = leftFist.transform.rotation;
//             Vector3 leftFistEuler = leftFistRotation.eulerAngles; 
            
//             leftFistEuler.y += (xDifference);
//             Debug.Log(leftFistEuler.y);
//             leftFistEuler.y = Mathf.Clamp(leftFistEuler.y, minX, maxX);
            
//             leftFistEuler.x -= (yDifference);
//             Debug.Log(leftFistEuler.x);
//             leftFistEuler.x = Mathf.Clamp(leftFistEuler.x, minY, maxY);
//             leftFist.transform.rotation = Quaternion.Euler(leftFistEuler);
            
//             //Debug.Log(leftFist.transform.position.x);

//             Quaternion rightFistRotation = rightFist.transform.rotation;
//             Vector3 rightFistEuler = rightFistRotation.eulerAngles; 
            
//             rightFistEuler.y += (xDifference);
//             rightFistEuler.y = Mathf.Clamp(rightFistEuler.y, minX, maxX);
            
//             rightFistEuler.x -= (yDifference);
//             rightFistEuler.x = Mathf.Clamp(rightFistEuler.x, minY, maxY);
//             rightFist.transform.rotation = Quaternion.Euler(rightFistEuler);

//         }

//         prevMouseX = mouseX;
//         prevMouseY = mouseY;
//     }
// }
