using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogCarAnimate : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] float framesPerSecond = 0.5f;
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    float timer = 0;
    int index = 0;
    void Update()
    {
        timer += Time.deltaTime * framesPerSecond;
        if (timer < 1)
            return;
        timer = 0;
        index++;
        if (index == sprites.Length)
            index = 0;
        spriteRenderer.sprite = sprites[index];

    }
}
