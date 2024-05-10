using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]bool trigger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
            SceneManager.LoadScene("MainMenu");
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}
