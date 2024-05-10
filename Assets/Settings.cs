using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public float volume = 1.0f;
    public Slider volumeSlider;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        AudioListener.volume = volume;
    }
    
    public void ChangeVolume()
    {
        if(volumeSlider != null) 
        {
            volume = volumeSlider.value;
            AudioListener.volume = volume;
        }
    }
}
