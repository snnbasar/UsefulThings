using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public static SoundControl instance;
    public AudioSource audioSource;
    public AudioClip[] foodClips;
    public AudioClip[] emptyBiteClips;

    private AudioClip oldPlayedClip;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    public void PlaySound(int soundIndex)
    {
        audioSource.PlayOneShot(foodClips[soundIndex]);
    }

    public void PlayTeethBrushSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(foodClips[3]);
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
    }

    public async void PlayRandomFoodSound(bool unique = false)
    {
        AudioClip clip = unique ? await foodClips.GetUniqueItem(oldPlayedClip) : foodClips.GetRandomItem();
        audioSource.PlayOneShot(clip);

        oldPlayedClip = clip;
    }

    public async void PlayRandomEmptyBiteSound(bool unique = false)
    {
        AudioClip clip = unique ? await emptyBiteClips.GetUniqueItem(oldPlayedClip) : emptyBiteClips.GetRandomItem();
        audioSource.PlayOneShot(clip);

        oldPlayedClip = clip;
    }
}
