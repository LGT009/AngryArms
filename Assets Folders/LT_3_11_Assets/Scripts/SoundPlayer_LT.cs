using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer_LT : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    AudioSource source; 
    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playEffect(int i){
        source.PlayOneShot(sounds[i]);
    }
}
