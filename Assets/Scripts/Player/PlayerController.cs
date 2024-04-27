using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float maxSpeed = 100;
    float defualtMaxSpeed;
    [SerializeField, Tooltip("How fast it gets up to max speed Bigger = faster")] float accelrationSpeed = 10;
    float acceleration = 0;
    [SerializeField,Tooltip("Speed at which the car rotates > = faster")] float rotationSpeed = 5;
    [SerializeField, Tooltip("how fast the car slows down when no buttens are beeing pressed > = faster")] float autoBreakSpeed = 5;
    [SerializeField, Tooltip("should be bigger than max speed to make it go faster")] float boostMaxSpeed = 200;
    [SerializeField, Tooltip("How long the boost lasts")] float boostDuration = 1;
    [SerializeField,Tooltip("starts after boost has finised")] float boostCooldown = 5;
    GameManager gameManager;
    [SerializeField]
    Sprite[] carSprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    float rotationDegrees;
    AudioSource audioSource;
    [SerializeField] RectTransform speedometer;
    [SerializeField] Image nitroImage;
    bool boostLatch;
    void Start()
    {
        defualtMaxSpeed = maxSpeed;
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (boostLatch)
                return;
            maxSpeed = boostMaxSpeed;
            StartCoroutine(BoostCooldown());
        }
        audioSource.pitch = acceleration / defualtMaxSpeed;
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
        float rot = Mathf.Lerp(65, -65, acceleration / boostMaxSpeed);
        speedometer.rotation = Quaternion.Euler(0,0,rot);

    }
    void FixedUpdate()
    {
        if(gameManager.isPaused)
            return;
        if(acceleration != 0)
        rb.velocity = transform.up * acceleration * Time.fixedDeltaTime;
    }
    IEnumerator BoostCooldown()
    {
        nitroImage.fillAmount = 0;
        boostLatch = true;
        float timer = 0;
        while(timer < boostDuration)
        {
            if (gameManager.isPaused)
                yield return null;
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        maxSpeed = defualtMaxSpeed;

        while(timer < boostCooldown)
        {
            if(gameManager.isPaused)
                yield return null;
            timer += Time.deltaTime;
            nitroImage.fillAmount = timer / boostCooldown;
            yield return null;
        }
        boostLatch = false;

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
