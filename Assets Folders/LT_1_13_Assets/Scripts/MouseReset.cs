using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Mouse.current.WarpCursorPosition(screenCenter);

        // Input.mousePosition.x = 0;
        // Input.mousePosition.y = 0;
        //Cursor.SetCursorPosition(0, 0);
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
