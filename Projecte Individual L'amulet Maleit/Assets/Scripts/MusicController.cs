using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioClip[] audioClips;

    public enum SoundType
    {
        Hurt,
        Slash,
        Death,
        // Afeigr més sfx a mesura que ho necesiti
    }

    private void PlayClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            sfxAudioSource.PlayOneShot(audioClip);
        }
    }

    private void PlaySound(SoundType soundType)
    {
        if (audioClips != null && audioClips.Length != 0)
        {
            switch (soundType) // CUIDADO: Assegurar que l'ordre dels clips correspon a l'inspector.
            {
                case SoundType.Hurt:
                    PlayClip(audioClips[0]);
                    break;
                case SoundType.Slash:
                    PlayClip(audioClips[1]);
                    break;
                case SoundType.Death:
                    PlayClip(audioClips[2]);
                    break;
                default:
                    Debug.Log("No existeix el tipus de so");
                    break;
            }
        }
    }

    public void PlayHurt()
    {
        PlaySound(SoundType.Hurt);
    }

    public void PlaySlash()
    {
        PlaySound(SoundType.Slash);
    }

    public void PlayDeath()
    {
        PlaySound(SoundType.Death);
    }
}
