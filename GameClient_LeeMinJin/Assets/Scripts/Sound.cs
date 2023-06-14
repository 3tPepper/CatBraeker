using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private static Sound _instance;
    public static Sound instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Sound>();
            }
            return _instance;
        }
    }

    //audio source
    public AudioSource camera_as;
    public AudioSource player_as;
    public AudioSource enemy_as;

    public AudioClip bgm;
    //player
    public AudioClip attack;
    public AudioClip defence;
    public AudioClip jump;

    //enemy
    public AudioClip enemy_die;

    private void Start()
    {
        //camera_as.PlayOneShot(bgm);
    }

    //player
    public void AtkSound()
    {
        player_as.PlayOneShot(attack);
    }

    public void DefSound()
    {
        player_as.PlayOneShot(defence);
    }

    public void JumpSound()
    {
        player_as.PlayOneShot(jump);
    }

    //enemy
    public void EnemySound()
    {
        enemy_as.PlayOneShot(enemy_die);
    }
}
