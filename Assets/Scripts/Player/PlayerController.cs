using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    Sprite[] carSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    float rotationDegrees;
    AudioSource audioSource;
    [SerializeField] RectTransform speedometer;
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

        SetSprite();

        audioSource.pitch = acceleration / maxSpeed;
        float degrees = (-Input.GetAxis("Horizontal") * (rotationSpeed * acceleration / maxSpeed)) * Time.deltaTime;
        rotationDegrees += degrees;
        transform.eulerAngles += new Vector3(0, 0, degrees);
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
        float rot = Mathf.Lerp(65, -65, acceleration / maxSpeed);
        speedometer.rotation = Quaternion.Euler(0,0,rot);

    }
    void FixedUpdate()
    {
        if(gameManager.isPaused)
            return;
        if(acceleration != 0)
        rb.velocity = transform.up * acceleration * Time.fixedDeltaTime;
    }


    void SetSprite()
    {
        if( rotationDegrees < 0)
        {
            rotationDegrees = 360;
        }
        else if(rotationDegrees > 360)
        {
            rotationDegrees = 0;
        }

         
        int index = (int)rotationDegrees / 15;
        index = Mathf.Clamp(index, 0, 23);
        spriteRenderer.sprite = carSprites[index];
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
