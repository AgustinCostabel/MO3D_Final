using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum State {
        Sunrise,
        Noon,
        Sunset,
        Night,
        DarkNight
    }

    private bool gamePaused = false;
    [SerializeField] private float gameTimerMax = 60f;
    private float gameTimer;
    private State state;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject winActivation;
    private bool menuOpened = false;

    public event EventHandler OnSunrise;
    public event EventHandler OnNoon;
    public event EventHandler OnSunset;
    public event EventHandler OnNight;
    public event EventHandler OnDarkNight;
    public event EventHandler OnTimeLapsed;

    private Player player;
    private LoseUI loseUI;
    private bool stopTimer;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE GAME MANAGER");
        }
        Instance = this;
    }

    private void Start() {
        Player.Instance.OnDeathPlayer += Player_OnDeathPlayer;

        gameTimer = gameTimerMax;

        //OnNight?.Invoke(this, EventArgs.Empty);   

        //MenuUI.Instance.SetTimelineActive(true);

        player = Player.Instance;

        stopTimer = true;
        MusicManager.Instance.StopSong();

        StartGame();
    }

    private void Player_OnDeathPlayer(object sender, EventArgs e) {
        MusicManager.Instance.StopSong();
        stopTimer = true;
    }

    void Update()
    {
        if (!stopTimer) {
            gameTimer -= Time.deltaTime;
        }

        if (gameTimer <= 0) {
            Win();
        }

        switch (state) {
            case State.Sunrise:
                if(gameTimer < gameTimerMax - gameTimerMax / 5) {
                    state = State.Noon;
                    OnNoon?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Noon:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 2) {
                    state = State.Sunset;
                    OnSunset?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Sunset:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 3) {
                    state = State.Night;
                    OnNight?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Night:
                if (gameTimer < gameTimerMax - gameTimerMax / 5 * 4) {
                    state = State.DarkNight;
                    OnDarkNight?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.DarkNight:
                if (gameTimer < 0) {
                    state = State.Sunrise;
                    OnSunrise?.Invoke(this, EventArgs.Empty);
                    gameTimer = gameTimerMax;
                }
                break;
        }

        //Timer text
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
    public void StartGame() {
        gameTimer = gameTimerMax;

        UnpauseGame();

        MusicManager.Instance.StopSong();

        OnTimeLapsed.Invoke(this, EventArgs.Empty);
        OnSunrise.Invoke(this, EventArgs.Empty);

        MenuUI.Instance.SetTimelineActive(false);
        Player.Instance.SetIsDoingAction(false);
    }

    public void Win() {
        PauseGame();
        MusicManager.Instance.StopSong();
        SoundManager.Instance.PlayVictory();
        winActivation.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartTimer() {
        stopTimer = false;
    }

    public void PauseGame() {
        gamePaused = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        gamePaused = false;
        Time.timeScale = 1f;
    }

    public float GetGameTimerMax() {
        return gameTimerMax;
    }

    public bool IsGamePaused() {
        return gamePaused;
    }

    public State GetGameState() {
        return state;
    }

    public bool GetMenuOpened() {
        return menuOpened;
    }

    public void SetMenuOpened(bool opened) {
        menuOpened = opened;
    }

    public bool GetTimer() {
        return stopTimer;
    }
}
