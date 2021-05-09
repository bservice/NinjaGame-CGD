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
    private GameObject mainMenuUI;
    private GameObject levelSelectUI;
    private GameObject pauseUI;
    private GameObject gameUI;
    private GameObject[] dashUI;
    private GameObject[] healthUI;
    public GameObject activated_d;
    public GameObject deactivated_d;
    public GameObject activated_h;
    public GameObject deactivated_h;
    private Player player;
    private Text gameScoreText;
    private Text gameTimerText;
    private Text scoreText;
    private Text highScoreText;
    private Text timerText;
    private Text bestTimeText;
    private Text completionStatusText;
    private int numberOfDashes;
    private int playerHealth;
    public float timePassed;
    public int score;
    public int scoreModifier;
    public int currentLevel;
    public int completionStatus; // 0 if player lost level, 1 if player won level
    public float screenWidth;
    public float screenHeight;
    private bool sceneNameParsed = false;
    private int totalLevels; // not including the tutorial

    string scoreKey;
    string highScoreKey;
    string timeKey;
    string bestTimeKey;

    // Start is called before the first frame update
    void Start()
    {
        // update the current state when a scene is loaded
        sceneNameParsed = Enum.TryParse(SceneManager.GetActiveScene().name, out currentState);
        
        // if the scene name does not match a game state, set it to the GameScene
        if (!sceneNameParsed)
        {
            currentState = GameState.GameScene;
        }
        
        screenHeight = Camera.main.orthographicSize;
        screenWidth = Camera.main.aspect * screenHeight;

        dashUI = new GameObject[3];
        healthUI = new GameObject[3];

        paused = false;
        score = 0;
        scoreModifier = 1;
        timePassed = 0;
        numberOfDashes = 3;
        totalLevels = 3;

        highScoreKey = "level0HighScore";
        bestTimeKey = "level0BestTime";

        // create high score and best time keys if there are none
        if (!PlayerPrefs.HasKey(highScoreKey))
        {
            for (int i = 0; i <= totalLevels; i++)
            {
                highScoreKey = "level" + i.ToString() + "HighScore";
                bestTimeKey = "level" + i.ToString() + "BestTime";
                PlayerPrefs.SetInt(highScoreKey, score);
                PlayerPrefs.SetFloat(bestTimeKey, timePassed);
            }
        }

        player = FindObjectOfType<Player>();

        Vector3 temp = new Vector3(0, 0, -2);

        if (player != null)
        {
            for (int i = 0; i < 3; i++)
            {
                dashUI[i] = Instantiate(activated_d, temp, Quaternion.identity);
                healthUI[i] = Instantiate(activated_h, temp, Quaternion.identity);

                temp.x += 0.4f;
            }
        }
        
        switch (currentState)
        {
            case GameState.MainMenuScene:
                mainMenuUI = GameObject.Find("MainMenuUI");
                levelSelectUI = GameObject.Find("LevelSelectUI");
                levelSelectUI.SetActive(false);

                // Uncomment the next 2 lines to reset your save file when the main menu scene is loaded
                //PlayerPrefs.DeleteAll();
                //PlayerPrefs.Save();
                break;
            case GameState.Tutorial:
                gameUI = GameObject.Find("GameUI");
                gameScoreText = GameObject.Find("ScoreNumberText").GetComponent<Text>();
                gameTimerText = GameObject.Find("TimerNumberText").GetComponent<Text>();
                pauseUI = GameObject.Find("PauseScreenUI");
                pauseUI.SetActive(false);
                currentLevel = 0;
                break;
            case GameState.GameScene:
                gameUI = GameObject.Find("GameUI");
                gameScoreText = GameObject.Find("ScoreNumberText").GetComponent<Text>();
                gameTimerText = GameObject.Find("TimerNumberText").GetComponent<Text>();
                pauseUI = GameObject.Find("PauseScreenUI");
                pauseUI.SetActive(false);
                currentLevel = PlayerPrefs.GetInt("currentLevel");
                break;
            case GameState.GameOverScene:
                currentLevel = PlayerPrefs.GetInt("currentLevel");
                completionStatus = PlayerPrefs.GetInt("completionStatus");

                scoreKey = "level" + currentLevel.ToString() + "Score";
                highScoreKey = "level" + currentLevel.ToString() + "HighScore";
                timeKey = "level" + currentLevel.ToString() + "Time";
                bestTimeKey = "level" + currentLevel.ToString() + "BestTime";

                score = PlayerPrefs.GetInt(scoreKey);
                timePassed = PlayerPrefs.GetFloat(timeKey);

                completionStatusText = GameObject.Find("CompletionStatusText").GetComponent<Text>();
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
                highScoreText = GameObject.Find("HighScoreText").GetComponent<Text>();
                timerText = GameObject.Find("TimeText").GetComponent<Text>();
                bestTimeText = GameObject.Find("BestTimeText").GetComponent<Text>();

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

                if (PlayerPrefs.GetInt(highScoreKey) > 0)
                {
                    highScoreText.text = "High Score: " + PlayerPrefs.GetInt(highScoreKey).ToString();
                }
                else
                {
                    highScoreText.text = "No High Score Yet";
                }
                
                timerText.text = "Time: " + TimeToString(timePassed);

                if (PlayerPrefs.GetFloat(bestTimeKey) > 0)
                {
                    bestTimeText.text = "Best Time: " + TimeToString(PlayerPrefs.GetFloat(bestTimeKey));
                }
                else
                {
                    bestTimeText.text = "No Best Time Yet";
                }
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
            case GameState.Tutorial:
            case GameState.GameScene:
                // pause when the player presses the ESCAPE key
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TogglePause();
                }

                // update the score UI
                gameScoreText.text = score.ToString();

                // track the time it takes for the player to complete the level
                if (!paused)
                {
                    timePassed += Time.deltaTime;
                    gameTimerText.text = TimeToString(timePassed);
                }

                break;
            case GameState.GameOverScene:

                // GAME OVER SCENE MANAGEMENT CODE HERE

                break;
        }

        float spacing = 0.4f;

        if (player != null)
        {
            Camera cam = Camera.main;
            float padding = 0.3f;

            for (int i = 0; i < dashUI.Length; i++)
            {
                dashUI[i].transform.position = new Vector3(cam.transform.position.x + screenWidth - padding - (.4f * i), cam.transform.position.y - screenHeight + padding, -2);
                healthUI[i].transform.position = new Vector3(cam.transform.position.x + screenWidth - padding - (.4f * i), cam.transform.position.y - screenHeight + padding + .4f, -2);
            }

            if (player.NumberOfDashes != numberOfDashes)
            {
                if (numberOfDashes - player.NumberOfDashes == -1)
                {
                    Vector3 position = dashUI[numberOfDashes].transform.position;

                    Destroy(dashUI[numberOfDashes]);

                    dashUI[numberOfDashes] = Instantiate(activated_d, position, Quaternion.identity);
                }

                else if (numberOfDashes - player.NumberOfDashes == 1)
                {
                    Vector3 position = dashUI[numberOfDashes - 1].transform.position;

                    Destroy(dashUI[numberOfDashes - 1]);

                    dashUI[numberOfDashes - 1] = Instantiate(deactivated_d, position, Quaternion.identity);
                }

                numberOfDashes = player.NumberOfDashes;
            }

            if (player.Health != playerHealth)
            {
                if (playerHealth - player.Health == -1)
                {
                    Vector3 position = healthUI[playerHealth].transform.position;

                    Destroy(healthUI[playerHealth]);

                    healthUI[playerHealth] = Instantiate(activated_h, position, Quaternion.identity);
                }

                else if (playerHealth - player.Health == 1)
                {
                    Vector3 position = healthUI[playerHealth - 1].transform.position;

                    Destroy(healthUI[playerHealth - 1]);

                    healthUI[playerHealth - 1] = Instantiate(deactivated_h, position, Quaternion.identity);
                }

                playerHealth = player.Health;
            }
        }
    }

    // Changes the scene to any given scene using GameStates
    public void ChangeScene(GameState nextState)
    {
        if (currentState == GameState.Tutorial || currentState == GameState.GameScene)
        {
            SaveGame();
        }

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
                if (currentLevel == 1 || currentLevel > totalLevels)
                {
                    SceneManager.LoadScene("GameScene");
                }
                else
                {
                    SceneManager.LoadScene("Level" + (currentLevel - 1).ToString());
                }
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

    // Goes to the next level
    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel > 1)
        {
            currentState = (GameState)(2);
        }
        else
        {
            currentState = (GameState)(currentLevel + 1);
        }

        ChangeScene(currentState);
    }

    // Play the current level again
    public void PlayAgain()
    {
        currentState = (GameState)(currentLevel + 1);
        ChangeScene(currentState);
    }

    // Closes the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Saves the game
    public void SaveGame()
    {
        scoreKey = "level" + currentLevel.ToString() + "Score";
        highScoreKey = "level" + currentLevel.ToString() + "HighScore";
        timeKey = "level" + currentLevel.ToString() + "Time";
        bestTimeKey = "level" + currentLevel.ToString() + "BestTime";

        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetInt("completionStatus", completionStatus);
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetFloat(timeKey, timePassed);

        // if the new score is greater than the old high score, save the high score
        if (score > PlayerPrefs.GetInt(highScoreKey) && completionStatus == 1)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
        }

        // if the new time is less than the old best time, save the best time
        if ((timePassed < PlayerPrefs.GetFloat(bestTimeKey) || PlayerPrefs.GetFloat(bestTimeKey) <= 0) && completionStatus == 1)
        {
            PlayerPrefs.SetFloat(bestTimeKey, timePassed);
        }

        PlayerPrefs.Save();
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

    // Modify the current score
    public void AddScore(int scoreChange)
    {
        score += scoreChange * scoreModifier;
    }

    // Toggles between the main menu and level select screen
    public void ToggleLevelSelect()
    {
        mainMenuUI.SetActive(!mainMenuUI.activeSelf);
        levelSelectUI.SetActive(!levelSelectUI.activeSelf);
    }

    // Selects a given level
    public void SelectLevel(int levelSelected)
    {
        currentLevel = levelSelected;
        currentState = GameState.GameScene;
        ChangeScene(currentState);
    }
}