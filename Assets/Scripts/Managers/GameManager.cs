using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    float timer;
    public int gameTimer = 0;
    public bool isPaused = false;
    public int exp;
    [SerializeField] int xpToLevelUp = 5;
    [SerializeField] float levelUpMultiplier;
    public int playerlevel = 0;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public delegate void PauseDelegate();
    public PauseDelegate pauseEvent;
    public PauseDelegate playEvent;
    public MusicSystem musicSystem;
    [HideInInspector] public AIManager aiManager;

    [Header("PickUpItems")]
    public int healthPacksAvaliable;
    [SerializeField] public GameObject healthPack;
    [SerializeField] int timeBetweenHealthPacks = 60;
    UpgradeManager upgradeManager;
    [Header("UI")]
    [SerializeField] Image expBarImage;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] TextMeshProUGUI textTime;
    float seconds, mins;
    AudioSource audioSource;
    AudioListener audioListener;
    [SerializeField]Slider slider;
    void Start()
    {
        aiManager = FindFirstObjectByType<AIManager>();
        upgradeManager = GetComponent<UpgradeManager>();
        audioListener = GetComponent<AudioListener>();
        pauseEvent += Pause;
        playEvent += Pause;
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    bool latch;

    public object DecreaseMusicIntensity { get; internal set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                playEvent();
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
                pauseEvent();
            }
        }
        if (isPaused)
            return;
        timer += Time.deltaTime;
        gameTimer = Mathf.FloorToInt(timer);
        seconds += Time.deltaTime;
        if (seconds > 60)
        {
            seconds = 0;
            mins++;
        }
        String text;
        if (seconds < 10)
            text = mins.ToString() + ":0" + Mathf.FloorToInt(seconds).ToString();
        else
            text = mins.ToString() + ":" + Mathf.FloorToInt(seconds).ToString();

        textTime.text = text;
        if (gameTimer % timeBetweenHealthPacks == 0)
        {
            latch = true;
        }
        else if (latch)
        {
            latch = false;
            healthPacksAvaliable++;
        }
    }
    public void DropHealthPack(Vector2 position)
    {
        --healthPacksAvaliable;
        Instantiate(healthPack, position, Quaternion.identity);
    }

    public void GiveExp(int xp)
    {
        exp += xp;
        expBarImage.fillAmount = (float)exp / (float)xpToLevelUp;
        if (exp >= xpToLevelUp)
        {
            exp = exp - xpToLevelUp;
            LevelUp();
            expBarImage.fillAmount = 0;
        }
    }
    void LevelUp()
    {
        audioSource.Play();
        playerlevel++;
        xpToLevelUp = (int)(xpToLevelUp * levelUpMultiplier);
        upgradeManager.UpgradeEvent();
    }
    public void EliteUpgradeEvent()
    {
        upgradeManager.UpgradeEvent();
    }

    void Pause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        playEvent();
    }

    public void DeathEvent()
    {
        pauseEvent();
        gameOverUI.SetActive(true);
    }
    public void WinEvent()
    {
        pauseEvent();
        gameWinUI.SetActive(true);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitApplication()
    {
        Application.Quit();
    }


    public void IncreaseMusicIntensity(int amount)
    {
        musicSystem.IncreaseIntensity(amount);
    }
    public void SetVolume()
    {
        AudioListener.volume = Mathf.Clamp01(slider.value);
    }
}
