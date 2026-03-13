using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerControl : MonoBehaviour
{
    int myMask;
    ManageGame GameSystem;
    // Start is called before the first frame update
    void Start()
    {
        GameSystem = ManageGame.GameSystem;
        myMask = this.gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameSystem.state == ManageGame.GameState.Game){
            this.gameObject.layer = myMask;
        } else{
            this.gameObject.layer = 0;
        }
        
    }
}
