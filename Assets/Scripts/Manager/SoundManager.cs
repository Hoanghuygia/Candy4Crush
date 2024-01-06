using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] destroyNoise;
    public AudioSource backgroundMusic;
    private Board board;
    private void Start() {
        board = FindAnyObjectByType<Board>();
    }
    void Update() {
        SetMusic();
    }
    public void PlayRandomDestroyNoise() {
        if(board != null) {
            if (board.sound) {
                int numberToUse = Random.Range(0, destroyNoise.Length);
                destroyNoise[numberToUse].Play();
            }
        }
    }
    private void SetMusic() {
        if(board != null) {
            if (!board.music) {
                backgroundMusic.mute = true;
            }
            else {
                backgroundMusic.mute = false;
            }
        }
    }
}
