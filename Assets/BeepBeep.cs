using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeepBeep : MonoBehaviour
{
    AudioSource audioSource;
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
        
    }
}
