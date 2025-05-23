using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [System.Serializable]
    public struct SFXEntry
    {
        public SFXEvent sfxEvent;
        public AudioClip clip;
    }

    public List<SFXEntry> sfxEntries = new List<SFXEntry>();

    private Dictionary<SFXEvent, AudioClip> sfxDict;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource uiSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Build dictionary
        sfxDict = new Dictionary<SFXEvent, AudioClip>();
        foreach (var entry in sfxEntries)
        {
            sfxDict[entry.sfxEvent] = entry.clip;
        }
        PlayMusic(SFXEvent.IntroMusicS);//temp, remove later
    }

    public void Play(SFXEvent evt)
    {
        if (sfxDict.ContainsKey(evt))
        {
            sfxSource.PlayOneShot(sfxDict[evt]);
        }
        else
        {
            Debug.LogWarning($"Sound for {evt} not assigned.");
        }
    }

    public void PlayMusic(SFXEvent evt, bool loop = true)
    {
        if (sfxDict.ContainsKey(evt))
        {
            musicSource.clip = sfxDict[evt];
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayUI(SFXEvent evt)
    {
        if (sfxDict.ContainsKey(evt))
        {
            uiSource.PlayOneShot(sfxDict[evt]);
        }
    }
}
