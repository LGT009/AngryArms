using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitiatePunch : MonoBehaviour
{

    [SerializeField] GameObject leftFist;
    [SerializeField] GameObject rightFist;

    public Animator Camera;
    public Animator LeftFistAnim;
    public Animator RightFistAnim;

    public float Thrust = 20f;
    
    private bool leftCanPunch = true; 
    private bool rightCanPunch = true; 

    private int leftCount = 0;
    private int rightCount = 0;

    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float punchInterval = 2f;

    public AnimationClip leftPunchClip;
    public AnimationClip rightPunchClip;

    private float leftStartTime = 0f;
    private float rightStartTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        LeftFistAnim.speed = minSpeed;
        RightFistAnim.speed = minSpeed;
    }

    // Update is called once per frame
    void Update() 
    {
    }
    
    public void OnLeftMouse(InputAction.CallbackContext ctx){
        if(leftCount == 0){
                if(ctx.performed)
                {   
                    leftStartTime = Time.time;
                }
                else if(ctx.canceled && leftCanPunch)
                {
                    leftCount = 1;
                    float pressDuration = Time.time - leftStartTime;
                    StartCoroutine(PlayLeftAnimationsAndWait(pressDuration));
                }
        }
        leftCount = 0;
    }

    public void OnRightMouse(InputAction.CallbackContext ctx){
        if(rightCount == 0){    
            if(ctx.performed)
            {   
                rightStartTime = Time.time;
            }
            else if(ctx.canceled && rightCanPunch)
            {
                rightCount = 1;
                float pressDuration = Time.time - rightStartTime;
                StartCoroutine(PlayRightAnimationsAndWait(pressDuration));
            }
        }
        rightCount = 0;
    }

    IEnumerator PlayLeftAnimationsAndWait(float duration)
    {
        leftCanPunch = false;
        rightCanPunch = false; 
        //Camera.Play("Punch_Shake", -1, 0f);  
        float speedMultiplier = Mathf.Clamp(minSpeed + duration, minSpeed, maxSpeed);
        LeftFistAnim.speed = speedMultiplier;
        LeftFistAnim.Play("RightFist_Punching", -1, 0f);  
        
        float waitTime = leftPunchClip.length;

        yield return new WaitForSeconds(waitTime/punchInterval);  
        LeftFistAnim.speed = minSpeed;
        rightCanPunch = true;
        yield return new WaitForSeconds(waitTime - waitTime/punchInterval);
        leftCanPunch = true;  
    }

    IEnumerator PlayRightAnimationsAndWait(float duration)
    {
        leftCanPunch = false;
        rightCanPunch = false;  
        //Camera.Play("Punch_Shake", -1, 0f);  
        float speedMultiplier = Mathf.Clamp(minSpeed + duration, minSpeed, maxSpeed);
        RightFistAnim.speed = speedMultiplier;
        RightFistAnim.Play("RightFist_Punching", -1, 0f);  

        float waitTime = rightPunchClip.length;

        yield return new WaitForSeconds(waitTime/punchInterval);  
        RightFistAnim.speed = minSpeed;
        leftCanPunch = true;  
        yield return new WaitForSeconds(waitTime - waitTime/punchInterval);
        rightCanPunch = true;
    }
}

// Use collision points to deal more damage, hit in senstive areas or hit in the same place over and over



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiatePunch : MonoBehaviour
{

    [SerializeField] GameObject leftFist;
    [SerializeField] GameObject rightFist;

    public Animator Camera;
    public Animator LeftFistAnim;
    public Animator RightFistAnim;
    public float Thrust = 20f;
    private bool leftCanPunch = true; 
    private bool rightCanPunch = true; 
    [SerializeField] bool playingLeftAnim = false;
    [SerializeField] bool playingRightAnim = false;
    //public int forceStrength = 10;

    // Start is called before the first frame update
    void Start()
    {
        LeftFistAnim.enabled = false;
        RightFistAnim.enabled = false;
    }

    // Update is called once per frame
    void Update() 
    {
        if(Input.GetMouseButtonDown(0) && leftCanPunch){
            LeftFistAnim.enabled = true;
            StartCoroutine(PlayLeftAnimationsAndWait());
        }
        if(Input.GetMouseButtonDown(1) && rightCanPunch){
            RightFistAnim.enabled = true;
            StartCoroutine(PlayRightAnimationsAndWait());
        }
    }

    IEnumerator PlayLeftAnimationsAndWait()
    {
        leftCanPunch = false;  
        playingLeftAnim = true;
        //Camera.Play("Punch_Shake", -1, 0f);  
        LeftFistAnim.Play("RightFist_Punching", -1, 0f);  

        yield return new WaitForSeconds(0.67f);  
        leftCanPunch = true;  
        playingLeftAnim = false;
        LeftFistAnim.enabled = false;
    }

    IEnumerator PlayRightAnimationsAndWait()
    {
        rightCanPunch = false;  
        playingRightAnim = true;
        //Camera.Play("Punch_Shake", -1, 0f);  
        RightFistAnim.Play("RightFist_Punching", -1, 0f);  

        yield return new WaitForSeconds(0.67f);  
        rightCanPunch = true;  
        playingRightAnim = false;
        RightFistAnim.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy")){
            if(playingLeftAnim == true){
                //other.gameObject.GetComponent<HEF_EnemyScript>().TakeDamage(1);
            

                Vector3 direction = other.gameObject.transform.position - transform.position; // Get direction vector

                direction.Normalize(); // Normalize to get unit vector

                //other.gameObject.GetComponent<Rigidbody>().AddForce(direction * forceStrength);

                other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Thrust);
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(Thrust, other.gameObject.transform.position, 5.0F, 3.0F);///
                
                //Debug.Log("Hit with left hand");

            }else if(playingRightAnim == true){
                //other.gameObject.GetComponent<HEF_EnemyScript>().TakeDamage(1);
            

                Vector3 direction = other.gameObject.transform.position - transform.position; // Get direction vector

                direction.Normalize(); // Normalize to get unit vector

                //other.gameObject.GetComponent<Rigidbody>().AddForce(direction * forceStrength);

                other.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Thrust);
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(Thrust, other.gameObject.transform.position, 5.0F, 3.0F);///
                
                //Debug.Log("Hit with right hand");

            }

        }
    }
}

*/