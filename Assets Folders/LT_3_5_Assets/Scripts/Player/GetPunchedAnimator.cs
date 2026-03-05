using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPunchedAnimator : MonoBehaviour
{
    [SerializeField] Health healthUI;
    [SerializeField] float baseDmgMult = 1f;
    float dmgMult;//damage dealt is dmgMult * fistSpeed
    //fistSpeed is about .8 without charge
    // Start is called before the first frame update
    void Start()
    {
        dmgMult = baseDmgMult;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("LFist")){
            InitiatePunch init = other.transform.parent.parent.parent.GetComponent<InitiatePunch>();
            //Debug.Log(init.lSpeed);
            //other.transform.GetComponent<Rigidbody>().isKinematic = false;
            if(init.leftHasHit == false && init.leftHasHit == false){
                float dmg = dmgMult*init.lSpeed;
                //Debug.Log(init.gameObject.name+ " L Punched " + dmg);
                healthUI.takeDamage(dmg);
                init.leftHasHit = true;
            }
            
        } else if(other.gameObject.CompareTag("RFist")){
            InitiatePunch init = other.transform.parent.parent.parent.GetComponent<InitiatePunch>();
            //Debug.Log(init.rSpeed);
            //other.transform.GetComponent<Rigidbody>().isKinematic = false;
            if(init.rightCanPunch == false && init.rightHasHit == false){
                float dmg = dmgMult*init.rSpeed;
                //Debug.Log(init.gameObject.name+" R Punched "+ dmg);
                healthUI.takeDamage(dmg);
                init.rightHasHit = true;
            }
           
        }
    }
    
}