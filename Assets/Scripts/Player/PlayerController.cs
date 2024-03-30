using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float maxSpeed = 100;
    [SerializeField] float accelrationSpeed = 10;
    [SerializeField] float acceleration = 0;
    [SerializeField] float rotationSpeed = 5;
    [SerializeField] float autoBreakSpeed = 5;
    GameManager gameManager;

    AudioSource audioSource;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        gameManager.pauseEvent += Pause;
        gameManager.playEvent += Resume;
    }
    private void Update()
    {
        if (gameManager.isPaused)
            return;
        audioSource.pitch = acceleration / maxSpeed;
        transform.eulerAngles += new Vector3(0, 0, (-Input.GetAxis("Horizontal") * (rotationSpeed * acceleration / maxSpeed)) *Time.deltaTime)  ;
        float vertInput = Input.GetAxisRaw("Vertical");

        if(vertInput != 0 )
        {
            acceleration = Mathf.Clamp( acceleration += vertInput * accelrationSpeed  * Time.deltaTime, -maxSpeed, maxSpeed);
           
        }
        else
        {
            if(acceleration < 0 )
            {
                acceleration -= acceleration * autoBreakSpeed * Time.deltaTime;
            }
            else if(acceleration > 0)
            {

                acceleration += (acceleration * autoBreakSpeed * Time.deltaTime) *-1;
            }
        }

    }
    void FixedUpdate()
    {
        if(gameManager.isPaused)
            return;
        if(acceleration != 0)
        rb.velocity = transform.up * acceleration * Time.fixedDeltaTime;
    }

    Vector2 pausedVel;
    void Pause()
    {
        pausedVel = rb.velocity;
        rb.velocity = Vector2.zero;
        audioSource.Pause();
    }

    void Resume()
    {
        audioSource.Play();
        rb.velocity = pausedVel;
    }
}
