using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class LazerProjectile : MonoBehaviour
{
    Vector2 positionOne;
    Vector2 positionTwo;
    float speed;
    int damage;
    [SerializeField]LineRenderer lineRenderer;
    [SerializeField]SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] frames;
    [SerializeField] float framesPerSec;
    int currentFrame;
    float timer;
    GameManager gameManager;
    public void Initilize(Vector2 pos1,  Vector2 pos2, float speed, int damage, GameManager gameManager)
    {
        positionOne = pos1;
        positionTwo = pos2;
        this.speed = speed; 
        this.damage = damage;
        this.gameManager = gameManager;
        pos2 = Vector2.Lerp(positionOne, positionTwo, lerpTime);
        pos1 = new Vector2(pos1.x, pos1.y + 20);
        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);
        spriteRenderer.transform.position = pos2;
    }
    Vector2 pos1;
    Vector2 pos2;
    float lerpTime;
    // Update is called once per frame
    void Update()
    {
        if (gameManager.isPaused)
            return;
        lerpTime += Time.deltaTime * speed;
        pos2 = Vector2.Lerp(positionOne, positionTwo, lerpTime);
        pos1 = new Vector2 (pos2.x, pos2.y + 200);
        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);
        spriteRenderer.transform.position = pos2;
        if(lerpTime >= 1)
        {
            Destroy(gameObject);
        }
    }
    public void Animate()
    {
        timer += Time.fixedDeltaTime;
        if (timer < framesPerSec)
            return;
        timer = 0f;

        if (currentFrame >= frames.Length)
            currentFrame = 0;
        spriteRenderer.sprite = frames[currentFrame];
        currentFrame += 1;
    }
}
