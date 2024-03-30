using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public EnemyInfo[] enemies;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyInfo enemy in enemies)
        {
            if (enemy == null)
                return;
            enemy.Animate();
        }
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }
}
