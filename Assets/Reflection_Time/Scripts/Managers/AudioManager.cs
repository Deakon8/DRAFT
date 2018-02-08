using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class in charge to play and control all sound in the game
/// </summary>
/// <remarks>
/// You can customize volume and sound as you want
/// Attached to AudioManager gameObject
/// </remarks>
public class AudioManager : PersistentSingleton<AudioManager> {

    [Range(0, 1)]
    public float sfxVolume;
    public static AudioClip kick { get; private set; }
    public static AudioClip ride { get; private set; }
    private AudioSource audioSource;

    //sound, which are player when white figures are hited
    private AudioClip[] whiteHits;
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        kick = Resources.Load<AudioClip>("Sounds/Kick");
        ride = Resources.Load<AudioClip>("Sounds/Ride");
        whiteHits = Resources.LoadAll<AudioClip>("Sounds/WhiteHit");
    }

    /// <summary>
    /// Play sound
    /// </summary>
    /// <param name="audioClip">played sound</param>
    public void PlayOneShot(AudioClip audioClip)
    {
        if (PlayerPrefsX.GetBool(Constants.PrefsIsSoundOn))
        {
            Debug.Log("<color=blue>PlayOneShot </color>");
            audioSource.PlayOneShot(audioClip, sfxVolume);
        }
    }

    /// <summary>
    /// Player sound like harmonic
    /// </summary>
    /// <param name="k">number of sequence</param>
    public void LadderPlay(int k)
    {
        if (PlayerPrefsX.GetBool(Constants.PrefsIsSoundOn))
        {
            k = Mathf.Clamp(k, 0, whiteHits.Length - 1);
            audioSource.PlayOneShot(whiteHits[k], sfxVolume);
        }
    } 
}
