using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerSplash : MonoBehaviour
{
    public bool sound;
    public bool music;
    public AudioSource clickNoise;
    public AudioSource backGroundMusic;
    public void PlayClickNoise() {
        if(sound) {
            clickNoise.Play();
        }
    }
    

}
