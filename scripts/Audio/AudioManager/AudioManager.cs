using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get ; private set;}
    private AudioSource source;
    private AudioSource musicSource;
    
    private void Awake() {
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this) {
            Destroy(gameObject);
        }

        changeVolume(0);
        musicVolume(0);    
    }

    public void playSound(AudioClip _sound) {
        source.PlayOneShot(_sound);
    }

    public void changeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source) {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            source.volume = 1;
        
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }

    public void changeVolume(float _change) {
        changeSourceVolume(1, "soundVolume", _change, source);
    }

    public void musicVolume(float _volume) {
        changeSourceVolume(0.3f, "musicVolume", _volume, musicSource);
    }
}
