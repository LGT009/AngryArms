using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPunchedAnimator : MonoBehaviour
{
    [SerializeField] Health healthUI;
    [SerializeField] float baseDmgMult = 1;
    float dmgMult;
    // Start is called before the first frame update
    void Start()
    {
        float dmgMult = baseDmgMult;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Fist")){
            Debug.Log("Punched");
            healthUI.takeDamage(dmgMult*other.transform.parent.parent.parent.GetComponent<InitiatePunch>().Thrust);
        }
    }
    
}