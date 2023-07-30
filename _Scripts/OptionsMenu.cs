using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
  

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVol", volume);
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVol", volume); 
    }
    public void SetEffectsVolume(float volume)
    {
        masterMixer.SetFloat("SFXVol", volume);
    }

    public void SetSensitivity(float sensitivity)
    {
        if (Settings.Instance != null)
        {
            Settings.Instance.setCameraSensitivity(sensitivity);
        }
        else
        {
            Debug.Log("Settings not yet initiated");
        }
    }
}
