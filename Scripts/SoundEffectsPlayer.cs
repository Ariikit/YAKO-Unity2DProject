using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;

    public void Jump()
    {
        src.clip = sfx1;
        src.Play();
    }
}
