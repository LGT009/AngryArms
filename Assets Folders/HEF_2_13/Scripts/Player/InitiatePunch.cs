using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InitiatePunch : MonoBehaviour
{
    [SerializeField] Rigidbody leftFistRb;
    [SerializeField] Rigidbody rightFistRb;
    [SerializeField] SpringJoint leftSpring;
    [SerializeField] SpringJoint rightSpring;
    [SerializeField] Transform leftFistModel; 
    [SerializeField] Transform rightFistModel;
    [SerializeField] Transform leftParent; 
    [SerializeField] Transform rightParent;

    [SerializeField] float idleSpring = 2000f;   
    [SerializeField] float idleDamper = 50f;
    [SerializeField] float punchSpring = 25f;   
    [SerializeField] float punchDamper = 5f;
    [SerializeField] float extensionTime = 0.12f; 

    public bool leftCanPunch = true;
    public bool rightCanPunch = true;
    public bool leftOppPause = false; 
    public bool rightOppPause = false;
    public bool leftCharge = false;
    public bool rightCharge = false;
    public bool leftHasHit = false; 
    public bool rightHasHit = false;

    public float lSpeed = 1f;
    public float rSpeed = 1f;
    public float lTimer = 0f;
    public float rTimer = 0f;
    public float lPressDuration = 0f;
    public float rPressDuration = 0f;
    
    [SerializeField] float punchCooldown = 1.0f; 
    public float oppPunchCooldown = 0.5f;
    public bool endRoundPause = false;

    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float durationToSpeedMult = 0.66f;
    [SerializeField] float forceMultiplier = 40f; 

    private float waitTime = 1.0f; 

    void Start()
    {
        SetJointState(leftSpring, idleSpring, idleDamper);
        SetJointState(rightSpring, idleSpring, idleDamper);
    }

    void Update() 
    {
        if(leftCharge && leftCanPunch) lPressDuration += Time.deltaTime;
        else if(!leftCharge) lPressDuration = 0f;
        
        leftOppPause = rTimer > (waitTime - oppPunchCooldown);
        
        if(lTimer > 0f)
        {
            lTimer -= Time.deltaTime;
            leftCanPunch = false;
        } 
        else 
        {
            leftCanPunch = true;
            leftHasHit = true; 
        }

        if(rightCharge && rightCanPunch) rPressDuration += Time.deltaTime;
        else if(!rightCharge) rPressDuration = 0f;

        rightOppPause = lTimer > (waitTime - oppPunchCooldown);
        
        if(rTimer > 0f)
        {
            rTimer -= Time.deltaTime;
            rightCanPunch = false;
        } 
        else 
        {
            rightCanPunch = true;
            rightHasHit = true; 
        }
    }
    
    public void OnLeftMouse(InputAction.CallbackContext ctx)
    {
        if(endRoundPause) return;

        if(ctx.performed)
        {   
            leftCharge = true;
        } 
        else if(ctx.canceled && (leftCanPunch && !leftOppPause))
        {
            StartCoroutine(ExecutePhysicsPunch(leftFistRb, leftSpring, leftParent, lPressDuration, true));
            leftCharge = false;
        }
    }

    public void OnRightMouse(InputAction.CallbackContext ctx)
    {
        if(endRoundPause) return;

        if(ctx.performed)
        {   
            rightCharge = true;
        }
        else if(ctx.canceled && (rightCanPunch && !rightOppPause))
        {
            StartCoroutine(ExecutePhysicsPunch(rightFistRb, rightSpring, rightParent, rPressDuration, false));
            rightCharge = false;
        }
    }

    IEnumerator ExecutePhysicsPunch(Rigidbody rb, SpringJoint joint, Transform parentTransform, float duration, bool isLeft) {
        float calculatedSpeed = Mathf.Clamp(minSpeed + (duration * durationToSpeedMult), minSpeed, maxSpeed);
        
        Transform targetModel = isLeft ? leftFistModel : rightFistModel;
        float xRot = 8;
        float yRot = -74;
        float zRot = -36;
        

        if(isLeft) {
            leftHasHit = false;
            lSpeed = calculatedSpeed;
            lTimer = punchCooldown / lSpeed;
        } else {
            rightHasHit = false;
            rSpeed = calculatedSpeed;
            rTimer = punchCooldown / rSpeed;
        }

        Vector3 punchDirection = parentTransform.forward;
        joint.maxDistance = 10f;
        SetJointState(joint, punchSpring, punchDamper);
        rb.velocity = Vector3.zero;
        rb.AddForce(punchDirection * forceMultiplier * calculatedSpeed, ForceMode.Impulse);

        float elapsed = 0;
        float punchTime = extensionTime / calculatedSpeed;

        while (elapsed < punchTime) {
            elapsed += Time.deltaTime;
            float t = elapsed / punchTime;
            float smoothedT = Mathf.SmoothStep(0, 1, t);
            targetModel.localRotation = Quaternion.Euler(smoothedT*xRot, smoothedT*yRot, smoothedT*zRot);
            yield return null;
        }

        joint.maxDistance = 0f;
        SetJointState(joint, idleSpring, idleDamper);

        elapsed = 0;
        while (elapsed < punchTime) {
            elapsed += Time.deltaTime;
            float t = 1-(elapsed / punchTime);
            float smoothedT = Mathf.SmoothStep(0, 1, t);
            targetModel.localRotation = Quaternion.Euler(smoothedT*xRot, smoothedT*yRot, smoothedT*zRot);
            yield return null;
        }
        
        targetModel.localRotation = Quaternion.identity;
    }

    IEnumerator FExecutePhysicsPunch(Rigidbody rb, SpringJoint joint, Transform parentTransform, float duration, bool isLeft) {
    float calculatedSpeed = Mathf.Clamp(minSpeed + (duration * durationToSpeedMult), minSpeed, maxSpeed);

    Transform targetModel = isLeft ? leftFistModel : rightFistModel;
    float targetRotation = -45;

    if(isLeft) {
        leftHasHit = false;
        lSpeed = calculatedSpeed;
        lTimer = punchCooldown / lSpeed;
    } else {
        rightHasHit = false;
        rSpeed = calculatedSpeed;
        rTimer = punchCooldown / rSpeed;
    }

    Vector3 punchDirection = parentTransform.forward;
    joint.maxDistance = 10f;
    SetJointState(joint, punchSpring, punchDamper);
    rb.velocity = Vector3.zero;
    rb.AddForce(punchDirection * forceMultiplier * calculatedSpeed, ForceMode.Impulse);

    float elapsed = 0;
    float punchTime = extensionTime / calculatedSpeed;

    while (elapsed < punchTime) {
        elapsed += Time.deltaTime;
        float t = elapsed / punchTime;
        targetModel.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, targetRotation, t));
        yield return null;
    }

    joint.maxDistance = 0f;
    SetJointState(joint, idleSpring, idleDamper);

    elapsed = 0;

    while (elapsed < punchTime) {
        elapsed += Time.deltaTime;
        float t = elapsed / punchTime;
        // Smoothly rotate back to 0
        targetModel.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(targetRotation, 0, t));
        yield return null;
    }
    
    // Ensure it resets perfectly
    targetModel.localRotation = Quaternion.Euler(0, 0, 0);
}


    void SetJointState(SpringJoint joint, float spring, float damper)
    {
        if(joint == null) return;
        joint.spring = spring;
        joint.damper = damper;
    }
}



// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InitiatePunch : MonoBehaviour
// {
//     [SerializeField] Rigidbody leftFistRb;
//     [SerializeField] Rigidbody rightFistRb;
//     [SerializeField] Transform leftParent; 
//     [SerializeField] Transform rightParent;

//     public bool leftCanPunch = true;
//     public bool rightCanPunch = true;
//     public bool leftOppPause = false; 
//     public bool rightOppPause = false;
//     public bool leftCharge = false;
//     public bool rightCharge = false;
//     public bool leftHasHit = false; 
//     public bool rightHasHit = false;

//     public float lSpeed = 1f;
//     public float rSpeed = 1f;
//     public float lTimer = 0f;
//     public float rTimer = 0f;
//     public float lPressDuration = 0f;
//     public float rPressDuration = 0f;
    
//     [SerializeField] float punchCooldown = 1.0f; 
//     public float oppPunchCooldown = 0.5f;

//     public bool endRoundPause = false;

//     [SerializeField] float minSpeed = 0.5f;
//     [SerializeField] float maxSpeed = 5f;
//     [SerializeField] float durationToSpeedMult = 0.66f;
//     [SerializeField] float forceMultiplier = 10f; 

//     private float waitTime = 1.0f; 

//     void Update() 
//     {
//         if(leftCharge && leftCanPunch) lPressDuration += Time.deltaTime;
//         else if(!leftCharge) lPressDuration = 0f;
        
//         leftOppPause = rTimer > (waitTime - oppPunchCooldown);
        
//         if(lTimer > 0f)
//         {
//             lTimer -= Time.deltaTime;
//             leftCanPunch = false;
//         } 
//         else 
//         {
//             leftCanPunch = true;
//             leftHasHit = true; 
//         }

//         if(rightCharge && rightCanPunch) rPressDuration += Time.deltaTime;
//         else if(!rightCharge) rPressDuration = 0f;

//         rightOppPause = lTimer > (waitTime - oppPunchCooldown);
        
//         if(rTimer > 0f)
//         {
//             rTimer -= Time.deltaTime;
//             rightCanPunch = false;
//         } 
//         else 
//         {
//             rightCanPunch = true;
//             rightHasHit = true; 
//         }
//     }
    
//     public void OnLeftMouse(InputAction.CallbackContext ctx)
//     {
//         if(endRoundPause) return;

//         if(ctx.performed)
//         {   
//             leftCharge = true;
//         } 
//         else if(ctx.canceled && (leftCanPunch && !leftOppPause))
//         {
//             ExecutePhysicsPunch(leftFistRb, leftParent, lPressDuration, true);
//             leftCharge = false;
//         }
//     }

//     public void OnRightMouse(InputAction.CallbackContext ctx)
//     {
//         if(endRoundPause) return;

//         if(ctx.performed)
//         {   
//             rightCharge = true;
//         }
//         else if(ctx.canceled && (rightCanPunch && !rightOppPause))
//         {
//             ExecutePhysicsPunch(rightFistRb, rightParent, rPressDuration, false);
//             rightCharge = false;
//         }
//     }

//     void ExecutePhysicsPunch(Rigidbody rb, Transform directionRef, float duration, bool isLeft)
//     {
//         float calculatedSpeed = Mathf.Clamp(minSpeed + (duration * durationToSpeedMult), minSpeed, maxSpeed);
        
//         if(isLeft) {
//             leftHasHit = false;
//             lSpeed = calculatedSpeed;
//             lTimer = punchCooldown / lSpeed;
//         } else {
//             rightHasHit = false;
//             rSpeed = calculatedSpeed;
//             rTimer = punchCooldown / rSpeed;
//         }

//         rb.velocity = Vector3.zero;

//         float punchForce = calculatedSpeed * forceMultiplier;
//         rb.AddForce(directionRef.forward * punchForce, ForceMode.Impulse);
//     }
// }





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InitiatePunch : MonoBehaviour
// {

//     [SerializeField] GameObject leftFist;
//     [SerializeField] GameObject rightFist;

//     public Animator Camera;
//     public Animator LeftFistAnim;
//     public Animator RightFistAnim;

//     public float Thrust = 20f;
    
//     public bool leftCanPunch = true; //getPunched Checks this //stops hand from punching if their own timer is on cooldown
//     public bool rightCanPunch = true; //getPunched Checks this

//     public bool leftOppPause = false; 
//     public bool rightOppPause = false;

//     public bool leftCharge = false;
//     public bool rightCharge = false;

//     public bool leftHasHit = false; //getPunched Checks this
//     public bool rightHasHit = false; //getPunched Checks this

//     public float lSpeed=1f;//getPunched Checks this
//     public float rSpeed=1f;//getPunched Checks this

//     public float lTimer=0f;//getPunched Checks this
//     public float rTimer=0f;//getPunched Checks this

//     public float lPressDuration=0f;
//     public float rPressDuration=0f;
//     [SerializeField] float punchCooldown;//multiplied by animation length
//     public float oppPunchCooldown;


//     public bool endRoundPause = false;//health Script WILL modify when round ends // not implemented yet
    

//     [SerializeField] float minSpeed = 0.5f;
//     [SerializeField] float maxSpeed = 5f;
//     [SerializeField] float durationToSpeedMult = 2/3f;

//     public AnimationClip leftPunchClip;
//     public AnimationClip rightPunchClip;

//     float waitTime;


//     // Start is called before the first frame update
//     void Start()
//     {
//         LeftFistAnim.speed = minSpeed;
//         RightFistAnim.speed = minSpeed;
//         waitTime = rightPunchClip.length;
//         punchCooldown *= waitTime;
//         oppPunchCooldown *= waitTime;
//     }

//     // Update is called once per frame
//     void Update() 
//     {
//         //Debug.Log("lTimer: "+lTimer+leftCanPunch+" rTimer: "+ rTimer + rightCanPunch);
//         //Debug.log(lTimer + " " + rTimer);
        
//         if(leftCharge){
//             if(leftCanPunch)lPressDuration += Time.deltaTime;
//         } else{
//             lPressDuration =0f;
//         }
        
//         leftOppPause = rTimer > (waitTime - oppPunchCooldown);//left fist cannot punch at start of right timer
//         if(lTimer>0f)
//         {
//             lTimer-=Time.deltaTime;
//             leftCanPunch = false;
//         } else {
//             leftCanPunch = true;
//             leftHasHit = true;
//         }

//         if(rightCharge){
//             if(rightCanPunch)rPressDuration += Time.deltaTime;
//         } else{
//             rPressDuration =0f;
//         }
//         rightOppPause = lTimer > (waitTime - oppPunchCooldown);
//         if(rTimer>0f)
//         {
//             rTimer-=Time.deltaTime;
//             rightCanPunch = false;
//         } else 
//         {
//             rightCanPunch = true;
//             rightHasHit = true;
//         }
//     }
    
//     public void OnLeftMouse(InputAction.CallbackContext ctx){
//         if(!endRoundPause){
//                 if(ctx.performed)
//                 {   
//                     leftCharge = true;
//                     //leftStartTime = Time.time;
//                 } else if(ctx.canceled && (leftCanPunch && !leftOppPause))
//                 {
//                     //float pressDuration = Time.time - leftStartTime;
//                     PlayLeftAnimationsAndWait(lPressDuration);
//                     leftCharge = false;
//                 }
//         }
//     }

//     public void OnRightMouse(InputAction.CallbackContext ctx){
//         if(!endRoundPause){    
//             if(ctx.performed)
//             {   
//                 rightCharge = true;
//                 //rightStartTime = Time.time;
//             }
//             else if(ctx.canceled && (rightCanPunch && !rightOppPause))
//             {
                
//                 //float pressDuration = Time.time - rightStartTime;
//                 PlayRightAnimationsAndWait(rPressDuration);
//                 rightCharge = false;
//             }
//         }
//     }

//     void PlayLeftAnimationsAndWait(float duration)
//     {
//         //leftCanPunch = false;
//         //rightCanPunch = false; 
//         //Camera.Play("Punch_Shake", -1, 0f);
//         leftHasHit = false;
        
//         //if(rTimer<oppPunchCooldown) rTimer = oppPunchCooldown;
//         lSpeed = Mathf.Clamp(minSpeed + (duration*durationToSpeedMult), minSpeed, maxSpeed);
//         LeftFistAnim.speed = lSpeed;
//         lTimer = punchCooldown / lSpeed;
//         LeftFistAnim.Play("RightFist_Punching", -1, 0f);
        
//         //Debug.Log(lSpeed);
        
//         //rightCanPunch = true;
//         //leftCanPunch = true;  
//     }

//     void PlayRightAnimationsAndWait(float duration)
//     {
//         //leftCanPunch = false;
//        // rightCanPunch = false;  
//         //Camera.Play("Punch_Shake", -1, 0f);
//         rightHasHit = false;
        
//         //if(lTimer<oppPunchCooldown)lTimer = oppPunchCooldown; 
//         rSpeed = Mathf.Clamp(minSpeed + (duration*durationToSpeedMult), minSpeed, maxSpeed);
//         RightFistAnim.speed = rSpeed;
//         rTimer = punchCooldown / rSpeed;
//         RightFistAnim.Play("RightFist_Punching", -1, 0f);  
//         //Debug.Log(rSpeed);
        
 
        
        
        
//         //leftCanPunch = true;
//         //rightCanPunch = true;
//     }
// }

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