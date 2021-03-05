using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneManagement : MonoBehaviour
{
    public enum GameState { MainMenuScene, GameScene, GameOverScene };
    public GameState currentState;
    public bool paused = false;
    public GameObject pauseUI;

    // Start is called before the first frame update
    void Start()
    {
        // update the current state when a scene is loaded
        Enum.TryParse(SceneManager.GetActiveScene().name, out currentState);

        paused = false;

        if(currentState == GameState.GameScene)
        {
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
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    TogglePause();
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
    }

    // Closes the game
    public void QuitGame()
    {
        Application.Quit();
    }
}