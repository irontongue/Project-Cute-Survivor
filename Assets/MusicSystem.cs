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
        public AudioClip drumClip;
    }

    [SerializeField] AudioSource mainPadLayer;
    [SerializeField] AudioSource drumLayer;
    [SerializeField] AudioSource arpLayer;
    [SerializeField] AudioSource bassLayer;
    [SerializeField] musicInfoPacket[] infoPackets;
    [SerializeField]int intensity = 0;
    void Start()
    {
        IncreaseIntensity();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            IncreaseIntensity();
        }
    }
    public void IncreaseIntensity()
    {
        StartCoroutine(WaitForClipEnd());
    }
    
    IEnumerator WaitForClipEnd()
    {
        while(arpLayer.time > 1)
        {
            print(arpLayer.time);
            yield return null;
        }
        print("It worked!");
        if (intensity == infoPackets.Length)
        {
            print("Tried To Increase Music Intesity Past Array Size");
        }
        else
        {
            musicInfoPacket packet = infoPackets[intensity];
            mainPadLayer.mute = !packet.mainPadLayer;
            drumLayer.mute = !packet.drumLayer;
            arpLayer.mute = !packet.arpLayer;
            bassLayer.mute = !packet.bassLayer;
            drumLayer.clip = packet.drumClip;
            drumLayer.Play();
            intensity++;
        }
    }

}
