using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HEF_AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip Menu;
    public AudioClip Level0_Tutorial_LGT;
    public AudioClip Level1_LGT;
    public AudioClip Level2_HEF;
    public AudioClip Level3_HEF;
    public AudioClip FistShoot;
    public AudioClip FistExplosion;

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        /*if(scene.name == "Menu"){
            musicSource.clip = Menu;
        }else if(scene.name == "Level0_Tutorial_LGT"){
            musicSource.clip = Level0_Tutorial_LGT;
        }else if(scene.name == "Level1_LGT"){
            musicSource.clip = Level1_LGT;
        }else if(scene.name == "Level2_HEF"){
            musicSource.clip = Level2_HEF;
        }else if(scene.name == "Level3_HEF"){
            musicSource.clip = Level3_HEF;
        }*/

        musicSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }
}
