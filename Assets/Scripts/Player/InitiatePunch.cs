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
    
    public bool leftCanPunch = true; //getPunched Checks this
    public bool rightCanPunch = true; //getPunched Checks this

    public float lSpeed=1f;//getPunched Checks this
    public float rSpeed=1f;//getPunched Checks this

    public float lTimer=0f;//getPunched Checks this
    public float rTimer=0f;//getPunched Checks this

    [SerializeField] float punchCooldown;
    public float oppPunchCooldown;//getPunched Checks this


    private int leftCount = 0;
    private int rightCount = 0;

    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float durationToSpeedMult = 2/3f;

    public AnimationClip leftPunchClip;
    public AnimationClip rightPunchClip;

    float waitTime;

    private float leftStartTime = 0f;
    private float rightStartTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        LeftFistAnim.speed = minSpeed;
        RightFistAnim.speed = minSpeed;
        waitTime = rightPunchClip.length;
    }

    // Update is called once per frame
    void Update() 
    {
        //Debug.Log("lTimer: "+lTimer+" rTimer: "+ rTimer + this.transform.parent.name);
        //Debug.log(lTimer + " " + rTimer);
        if(lTimer>0f)
        {
            lTimer-=Time.deltaTime;
            leftCanPunch = false;
        } else {
            leftCanPunch = true;
        }
        if(rTimer>0f)
        {
            rTimer-=Time.deltaTime;
            rightCanPunch = false;
        } else 
        {
            rightCanPunch = true;
        }
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
                    PlayLeftAnimationsAndWait(pressDuration);
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
                PlayRightAnimationsAndWait(pressDuration);
            }
        }
        rightCount = 0;
    }

    void PlayLeftAnimationsAndWait(float duration)
    {
        //leftCanPunch = false;
        //rightCanPunch = false; 
        //Camera.Play("Punch_Shake", -1, 0f);  
        lTimer = punchCooldown;
        if(rTimer<oppPunchCooldown) rTimer = oppPunchCooldown;
        lSpeed = Mathf.Clamp(minSpeed + (duration*durationToSpeedMult), minSpeed, maxSpeed);
        LeftFistAnim.speed = lSpeed;
        LeftFistAnim.Play("RightFist_Punching", -1, 0f);  
        //Debug.Log(lSpeed);
        
        //rightCanPunch = true;
        //leftCanPunch = true;  
    }

    void PlayRightAnimationsAndWait(float duration)
    {
        //leftCanPunch = false;
       // rightCanPunch = false;  
        //Camera.Play("Punch_Shake", -1, 0f);
        
        rTimer = punchCooldown;
        if(lTimer<oppPunchCooldown)lTimer = oppPunchCooldown; 
        rSpeed = Mathf.Clamp(minSpeed + (duration*durationToSpeedMult), minSpeed, maxSpeed);
        RightFistAnim.speed = rSpeed;
        RightFistAnim.Play("RightFist_Punching", -1, 0f);  
        //Debug.Log(rSpeed);
        
 
        
        
        
        //leftCanPunch = true;
        //rightCanPunch = true;
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