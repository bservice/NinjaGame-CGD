using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneManagement : MonoBehaviour
{
    public enum GameState { MainMenuScene, GameScene, GameOverScene };
    public GameState currentState;
    public bool paused = false;
    private GameObject pauseUI;
    private GameObject gameUI;
    private Text scoreText;
    private Text timerText;
    public float timePassed;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        // update the current state when a scene is loaded
        Enum.TryParse(SceneManager.GetActiveScene().name, out currentState);

        paused = false;
        score = 0;
        timePassed = 0;

        if(currentState == GameState.GameScene)
        {
            gameUI = GameObject.Find("GameUI");
            scoreText = GameObject.Find("ScoreNumberText").GetComponent<Text>();
            timerText = GameObject.Find("TimerNumberText").GetComponent<Text>();
            pauseUI = GameObject.Find("PauseScreenUI");
            pauseUI.SetActive(false);
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

                    if (timePassed < 60)
                    {
                        timerText.text = Math.Truncate(timePassed % 60).ToString();
                    }
                    else
                    {
                        timerText.text = Math.Truncate(timePassed / 60).ToString() + ":" + Math.Truncate(timePassed % 60).ToString("00");
                    }
                }

                break;
            case GameState.GameOverScene:

                // GAME OVER SCENE MANAGEMENT CODE HERE

                break;
        }
    }

    // Changes the scene to any given scene using GameStates
    public void ChangeScene(GameState nextState)
    {
        switch (nextState)
        {
            case GameState.MainMenuScene:
                SceneManager.LoadScene("MainMenuScene");
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
        SceneManager.LoadScene(nextState);
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
}