using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<AudioManager>();
            }
            return instance;
        }
    }

    [SerializeField] AudioMixer m_MasterMixer;
    public void SetBGMAttenuation(float attenuation)
    {
        m_MasterMixer.SetFloat("BGMVOLUME", attenuation);
    }
    public void SetSFXAttenuation(float attenuation)
    {
        m_MasterMixer.SetFloat("SFXVOLUME", attenuation);
    }
}
