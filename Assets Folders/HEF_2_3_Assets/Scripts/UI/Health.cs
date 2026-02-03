using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Image healthBar;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float health = 100;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = GetComponent<Image>();
        healthBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health/maxHealth;
        //health-= Time.deltaTime;
    }
    
    public void takeDamage(float damage){
        //Check Speed -- How?
            //Check current thrust
        //Debug.Log("takeDamage: " + damage);
        health -= damage;
    }
    public float getHealth(){
        return health;
    }

    public bool isDead(){
        return(health<=0f);
    }
    
}