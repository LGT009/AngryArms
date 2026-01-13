using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    [Range(0,1)] public float defaultSpeed;
    [Range(0,5)] public float cheeringSpeed;
    [Range(0, 1)] public float randomnessFactor;

    public float scaleFactor = 1f;

    public float maximumHeight;
    [HideInInspector] public float currentSpeedFactor;

    private float timer = 0f;

    private void Awake()
    {
        currentSpeedFactor = defaultSpeed;
    }

    private void Update(){
        timer += Time.deltaTime;
        if((int)(timer%4) == 0){
            UpdateState("Cheer");
        }
    }

    private void UpdateState(string state)
    {
        switch(state)
        {
            case "Idle":
                currentSpeedFactor = defaultSpeed;
                //Set the speed to default value

                break;
            case "Cheer":
                currentSpeedFactor = defaultSpeed;
                //Set the speed to cheering value
                //Here you can play anything, maybe a cheering sound, fireworks, animations 
                break; 
        }
    }
}
