using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public List<SoundInfo> sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundsType type, float volume = 1f)
    {
        var info = sounds.FirstOrDefault(sound => sound.type == type);
        audioSource.PlayOneShot(info.sound, volume);
    }

    [Serializable]
    public struct SoundInfo
    {
        public AudioClip sound;
        public SoundsType type;
     
    }

    public enum SoundsType
    {
        Flipping,
        Matching,
        Mismatching,
        GameOver
    }
}
