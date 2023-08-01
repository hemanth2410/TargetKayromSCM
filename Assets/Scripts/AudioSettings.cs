using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public void setSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXAttenuation(value);
    }
    public void setBGMVolume(float value) 
    {
        AudioManager.Instance.SetBGMAttenuation(value);
    }

}
