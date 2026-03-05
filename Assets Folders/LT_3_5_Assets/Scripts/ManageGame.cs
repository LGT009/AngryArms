using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ManageGame : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] virtCams;
    [SerializeField] Camera[] cams;
    [SerializeField] Canvas[] canvas;
    LayerMask[] masks = new LayerMask[3];

    float toGameTime = 2f;
    
    public static ManageGame GameSystem;//reference to my gamemanager object

    float timer = 0;
    // Start is called before the first frame update
    public GameState state = GameState.MainMenu;
    public enum GameState{
        MainMenu,
        Tutorial,
        Settings,
        ToGame,
        Game,
        ToMenu,
    }

    void Awake(){
        GameSystem = this;
    }
    void Start()
    {
        for(int i = 0;i<cams.Length;i++)
        {
            masks[i] = cams[i].cullingMask;
        }
        swapToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0f){
            timer-= Time.deltaTime;
        } else if (state == GameState.ToGame){
            state = GameState.Game;

            /*for(int i = 1;i<cams.Length;i++)
            {
                cams[i].cullingMask = masks[i];
            }*/
        }
    }
    public void swapToMenu(){
        virtCams[0].enabled = true;
        for(int i = 1;i<3;i++)
        {
            virtCams[i].enabled = false;

        }
        cams[0].enabled = true;
        for(int i = 1;i<3;i++)
        {
            cams[i].enabled = false;
            //cams[i].cullingMask = masks[0];
        }

        canvas[0].enabled = true;//Enable Main Menu
        
        canvas[1].enabled = false;//Disable player canvases
        //Reset Players
        //Reset Health
        //Music
    }
    public void swapToGame(){//Called when Start Game button is pressed
        
        for(int i = 1;i<3;i++)
        {
            virtCams[i].enabled = true;
        }
        virtCams[0].enabled = false;
        
        for(int i = 1;i<3;i++)
        {
            cams[i].enabled = true;
        }
        cams[0].enabled = false;
        
        canvas[0].enabled = false;//Disable Main Menu
        canvas[1].enabled = true;//Enable player canvases
        
        //Reset Players
        //Reset Health
        //Music
        timer = toGameTime;
        state= GameState.ToGame;
        
    }
    public void swapToTutorial(){//Called when Tutorial Button Pressed
        
        canvas[0].enabled = false;
        for(int i = 1;i<3;i++)
        {
            canvas[i].enabled = true;
        }

        canvas[3].enabled = false;//Enable Tutorial Canvas
        
        //Reset Players
        //Reset Health
        //Music
        timer = toGameTime;
        state= GameState.ToGame;
        
    }

    
}
