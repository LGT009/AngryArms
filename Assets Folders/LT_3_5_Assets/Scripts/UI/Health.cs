using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    ManageGame GameSystem;

    Image healthBar;
    int lives = 3;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float health = 100;
    [SerializeField] Image[] icons = new Image[2];
    [SerializeField] GameObject player;
    [SerializeField] Health oppHealth;
    GameObject body;
    GameObject head;
    InitiatePunch punchScript;
    float koTime = 3f;
    float timer=0f;
    bool dead = false;
    // Start is called before the first frame update
    Vector3 startPos;
    Quaternion startRot;
    Quaternion startHeadRot;

    void Start()
    {
        GameSystem = ManageGame.GameSystem;
        health = maxHealth;
        healthBar = GetComponent<Image>();
        healthBar.fillAmount = 1;

        body = player.transform.GetChild(0).gameObject;
        head = body.transform.GetChild(0).gameObject;
        
        punchScript = player.transform.GetChild(1).gameObject.GetComponent<InitiatePunch>();

        startPos = body.transform.position;
        startRot = body.transform.rotation;
        startHeadRot = head.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health/maxHealth;
        //health-= Time.deltaTime;
        
    }
    void FixedUpdate(){
        if(GameSystem.state == ManageGame.GameState.ToGame)
        {
            oppHealth.fullReset(); fullReset();
        }
        if(timer>0f) timer -= Time.deltaTime;
        
        if(timer<0f && dead){
            if(lives == 0){gameLost();}//Need to have game win screen and send to main menu
            else if(lives>0){oppHealth.resetTransform(); fightReset();}
        } else if (timer<0f && !dead){oppHealth.unpausePunch();unpausePunch();}
        
    }
    
    public void takeDamage(float damage){
        //Check Speed -- How?
            //Check current thrust
        //Debug.Log("takeDamage: " + damage);
        health -= damage;
        if(health<=0f && !dead && lives>0) die();
    }

    
    void die(){
        dead = true;
        lives -=1;
        //icons[lives-2].SetEnabled(false);
        if(lives > 0) icons[lives-1].enabled = false;
        
        //Set Connected body to none, letting the head pop off
        body.GetComponent<ConfigurableJoint>().connectedBody = null;
        
        //Start timer to let head roll around
        //Enable opp KO color
        pausePunch();
        oppHealth.pausePunch();
        timer = koTime;
        
    }

    void gameLost(){
        //Other player wins!
        //Enable opp KO color
        //start timer

        fullReset();
        oppHealth.fullReset();
        GameSystem.swapToMenu();
        
    }
    public void fullReset(){
        //Disable KO img
        //Disable Player wins
        foreach(Image icon in icons)
        {
            //icon.SetEnabled(true);
            icon.enabled =true;
        }
        lives = 3;
        fightReset();//temp reset
        
        //disable game canvas
        //Win screen?
        //Back to title screen?
        
    }
    void fightReset(){
        //reset players, head, and set health to max
        resetTransform();
        
        //Disable KO img


        body.GetComponent<ConfigurableJoint>().connectedBody = head.GetComponent<Rigidbody>();
        
        //reset Own Health
        health = maxHealth;
        
    }

    public void resetTransform(){
        
        player.transform.position =startPos;
        player.transform.rotation =startRot;
        head.transform.rotation= startHeadRot;
        
        dead = false;
    }
    public void setHealth(float hp){
        health = hp;
    }
    public void unpausePunch(){
        punchScript.endRoundPause = false;
    }

    public void pausePunch(){
        punchScript.endRoundPause = true;
    }
}