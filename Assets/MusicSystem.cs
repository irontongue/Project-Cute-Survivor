using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicSystem : MonoBehaviour
{
    [System.Serializable]
    public struct musicInfoPacket 
    {
        public bool mainPadLayer;
        public bool drumLayer;
        public bool arpLayer;
        public bool bassLayer;
        public bool leadLayer; // testing adding in lead instrument
        public AudioClip drumClip;
        public AudioClip arpClip;
        public AudioClip bassClip;
        public AudioClip leadClip; // testing the ability to change loops
    }

    [SerializeField] AudioSource mainPadLayer;
    [SerializeField] AudioSource drumLayer;
    [SerializeField] AudioSource arpLayer;
    [SerializeField] AudioSource bassLayer;
    [SerializeField] AudioSource leadLayer; // testing adding in lead instrument
    [SerializeField] musicInfoPacket[] infoPackets;
    [SerializeField] int intensity = 0;
    void Start()
    {
        IncreaseIntensity(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            //IncreaseIntensity();
        }
    }
    public void IncreaseIntensity(int amount)
    {
        StartCoroutine(WaitForClipEnd(amount));
    }
    
    IEnumerator WaitForClipEnd(int amount)
    {
        while(arpLayer.time > 1)
        {
            yield return null;
        }
        if (intensity == infoPackets.Length)
        {
            print("Tried To Increase Music Intesity Past Array Size");
        }
        else
        {
            intensity += amount;
            musicInfoPacket packet = infoPackets[intensity];
            mainPadLayer.mute = !packet.mainPadLayer;
            drumLayer.mute = !packet.drumLayer;
            arpLayer.mute = !packet.arpLayer;
            bassLayer.mute = !packet.bassLayer;
            leadLayer.mute = !packet.leadLayer; // testing adding in lead instrument
            drumLayer.clip = packet.drumClip;
            drumLayer.Play();
            arpLayer.clip = packet.arpClip;
            arpLayer.Play();
            bassLayer.clip = packet.bassClip;
            bassLayer.Play();
            leadLayer.clip = packet.leadClip;
            leadLayer.Play();
        }
    }

}
