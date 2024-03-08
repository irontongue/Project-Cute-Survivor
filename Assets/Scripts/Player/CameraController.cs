using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        transform.position = pos;
        
    }
}
