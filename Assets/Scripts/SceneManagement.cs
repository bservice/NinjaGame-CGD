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
    private GameObject[] dashUI;
    public GameObject activated;
    public GameObject deactivated;
    private Player player;
    private Text scoreText;
    private Text timerText;
    private int numberOfDashes;
    public float timePassed;
    public int score;

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

        for (int i = 0; i < 3; i++)
        {
            dashUI[i] = Instantiate(activated, temp, Quaternion.identity);

            temp.x += 0.4f;
        }

        if (currentState == GameState.GameScene)
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

        float spacing = 0.4f;

        for(int i = 0; i < dashUI.Length; i++)
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

            else if(numberOfDashes - player.NumberOfDashes == 1)
            {
                Vector3 position = dashUI[numberOfDashes - 1].transform.position;

                Destroy(dashUI[numberOfDashes - 1]);

                dashUI[numberOfDashes - 1] = Instantiate(deactivated, position, Quaternion.identity);
            }

            numberOfDashes = player.NumberOfDashes;
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