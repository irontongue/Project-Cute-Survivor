using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeepBeep : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip clip;
    [SerializeField, Range(0,1)] float volume = 1;
    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
