using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerSplash : MonoBehaviour
{
    public AudioSource clickNoise;
    public void PlayClickNoise() {
        clickNoise.Play();
    }
}
