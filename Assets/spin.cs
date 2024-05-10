using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 2;
    // Start is called before the first frame update
    Vector3 angle = new Vector3(0, 0, 1);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z + 1 * speed);
    }
}
