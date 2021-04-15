using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneManagement : MonoBehaviour
{
    public enum GameState { MainMenuScene, Tutorial, GameScene, GameOverScene };
    public GameState currentState;
    public bool paused = false;
    private GameObject pauseUI;
    private GameObject gameUI;
    private GameObject[] dashUI;
    public GameObject activated;
    public GameObject deactivated;
    private Player player;
    private Text scoreText;
    private Text timerText;
    private Text completionStatusText;
    private int numberOfDashes;
    public float timePassed;
    public int score;
    public int currentLevel;
    public int completionStatus; // 0 if player lost level, 1 if player won level

    // Start is called before the first frame update
    void Start()
    {
        // update the current state when a scene is loaded
        Enum.TryParse(SceneManager.GetActiveScene().name, out currentState);

        dashUI = new GameObject[3];

        paused = false;
        score = 0;
        timePassed = 0;
        numberOfDashes = 3;

        player = FindObjectOfType<Player>();

        Vector3 temp = new Vector3(0, 0, -2);

        if (player != null)
        {
            for (int i = 0; i < 3; i++)
            {
                dashUI[i] = Instantiate(activated, temp, Quaternion.identity);

                temp.x += 0.4f;
            }
        }

        switch (currentState)
        {
            case GameState.MainMenuScene:
                break;
            case GameState.Tutorial:
                currentLevel = 0;
                break;
            case GameState.GameScene:
                gameUI = GameObject.Find("GameUI");
                scoreText = GameObject.Find("ScoreNumberText").GetComponent<Text>();
                timerText = GameObject.Find("TimerNumberText").GetComponent<Text>();
                pauseUI = GameObject.Find("PauseScreenUI");
                pauseUI.SetActive(false);
                currentLevel = 1;
                break;
            case GameState.GameOverScene:
                currentLevel = PlayerPrefs.GetInt("currentLevel");
                completionStatus = PlayerPrefs.GetInt("completionStatus");

                string scoreKey = "level" + currentLevel.ToString() + "Score";
                string timeKey = "level" + currentLevel.ToString() + "Time";

                score = PlayerPrefs.GetInt(scoreKey);
                timePassed = PlayerPrefs.GetFloat(timeKey);

                completionStatusText = GameObject.Find("CompletionStatusText").GetComponent<Text>();
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
                timerText = GameObject.Find("TimeText").GetComponent<Text>();

                // player lost level
                if (completionStatus == 0)
                {
                    completionStatusText.text = "Game Over";
                }
                // player won level
                else if (completionStatus == 1)
                {
                    completionStatusText.text = "Level Complete!";
                }
                scoreText.text = "Score: " + score.ToString();
                timerText.text = "Time: " + TimeToString(timePassed);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.MainMenuScene:

                // MAIN MENU SCENE MANAGEMENT CODE HERE

                break;
            case GameState.GameScene:
                // pause when the player presses the ESCAPE key
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TogglePause();
                }

                // update the score UI
                scoreText.text = score.ToString();

                // track the time it takes for the player to complete the level
                if (!paused)
                {
                    timePassed += Time.deltaTime;

                    timerText.text = TimeToString(timePassed);
                }

                break;
            case GameState.GameOverScene:

                // GAME OVER SCENE MANAGEMENT CODE HERE

                break;
        }

        float spacing = 0.4f;

        if (player != null)
        {
            for (int i = 0; i < dashUI.Length; i++)
            {
                dashUI[i].transform.position = new Vector3(player.transform.position.x + (spacing * i) + 10, player.transform.position.y - 6.5f, -2);
            }

            if (player.NumberOfDashes != numberOfDashes)
            {
                if (numberOfDashes - player.NumberOfDashes == -1)
                {
                    Vector3 position = dashUI[numberOfDashes].transform.position;

                    Destroy(dashUI[numberOfDashes]);

                    dashUI[numberOfDashes] = Instantiate(activated, position, Quaternion.identity);
                }

                else if (numberOfDashes - player.NumberOfDashes == 1)
                {
                    Vector3 position = dashUI[numberOfDashes - 1].transform.position;

                    Destroy(dashUI[numberOfDashes - 1]);

                    dashUI[numberOfDashes - 1] = Instantiate(deactivated, position, Quaternion.identity);
                }

                numberOfDashes = player.NumberOfDashes;
            }
        }
    }

    // Changes the scene to any given scene using GameStates
    public void ChangeScene(GameState nextState)
    {
        // save game before switching scenes
        string scoreKey = "level" + currentLevel.ToString() + "Score";
        string timeKey = "level" + currentLevel.ToString() + "Time";

        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("completionStatus", completionStatus);
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetFloat(timeKey, timePassed);
        PlayerPrefs.Save();

        // switch scenes
        switch (nextState)
        {
            case GameState.MainMenuScene:
                SceneManager.LoadScene("MainMenuScene");
                break;
            case GameState.Tutorial:
                SceneManager.LoadScene("Tutorial");
                break;
            case GameState.GameScene:
                SceneManager.LoadScene("GameScene");
                break;
            case GameState.GameOverScene:
                SceneManager.LoadScene("GameOverScene");
                break;
        }
    }

    // Changes the scene to any given scene using strings
    public void ChangeScene(string nextState)
    {
        Enum.TryParse(nextState, out GameState nextGameState);
        ChangeScene(nextGameState);
    }

    // Pauses and unpauses the game when in the Game scene
    public void TogglePause()
    {
        paused = !paused;
        pauseUI.SetActive(paused);
        gameUI.SetActive(!gameUI.activeSelf);
    }

    // Closes the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Formats floats that represent time into a readable string
    public string TimeToString(float timeValue)
    {
        string timeString;

        if (timeValue < 60)
        {
            timeString = Math.Truncate(timeValue % 60).ToString();
        }
        else
        {
            timeString = Math.Truncate(timeValue / 60).ToString() + ":" + Math.Truncate(timeValue % 60).ToString("00");
        }

        return timeString;
    }
}