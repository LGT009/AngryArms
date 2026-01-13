using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{

    private Crowd crowd;

    private float angle; 
    private float startingYPosition;
    private float yOffset;
    private float randomSpeed;

    private float initialY;


    // Start is called before the first frame update
    private void Start()
    {
        crowd = FindObjectOfType<Crowd>();

        startingYPosition = transform.position.y;
        randomSpeed= Random.Range(crowd.defaultSpeed - crowd.randomnessFactor,
            crowd.defaultSpeed + crowd.randomnessFactor);

        yOffset = startingYPosition + crowd.maximumHeight;

        initialY = transform.position.y;

        ChooseRandomColor();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
    
        angle += crowd.currentSpeedFactor + randomSpeed/10;
        angle = angle/crowd.scaleFactor;

        Vector3 newPos = new Vector3(transform.position.x, 
            initialY + Mathf.Sin(angle)/crowd.maximumHeight,
            transform.position.z);
        transform.position = newPos;

        //Debug.Log(initialY + Mathf.Sin(angle));


    }

    private void ChooseRandomColor(){
        Renderer renderer = GetComponent<Renderer>();
        Material newMaterial = renderer.material;

        // newMaterial.color = new Color(Random.Range(0, 256) / 255f, 
        //     Random.Range(0, 256) / 255f,
        //     Random.Range(0, 256) / 255f);
        newMaterial.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); // Example: Value from 0.7 to 1 for lighter colors
        newMaterial.SetFloat("_Smoothness", 0.18f);

        renderer.material = newMaterial;
    }
}
