using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Audio from zapsplat.com

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips = new List<AudioClip>();

    public void PlayAudioClip(int audioClipNumber)
    {
        Debug.Log("Playing audio clip # " + audioClipNumber);
        audioSource.clip = audioClips[audioClipNumber];
        audioSource.Play();
    }

}
