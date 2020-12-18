using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    [SerializeField] public FpsMovement player;
    [SerializeField] private Text timeLabel;
    [SerializeField] private Text scoreLabel;
    [SerializeField] public Text eolText;
    [SerializeField] public UnityEngine.UI.Image bg;
    public int sizeRow;
    public int sizeCol;
    public NavMeshSurface surface;


    private MazeConstructor generator;

    private DateTime startTime;
    private int timeLimit;
    private int reduceLimitBy;

    private int score;
    private bool goalReached;

    // Use this for initialization
    void Start() {
        sizeRow = MainMenu.rowChoice > 0 ? MainMenu.rowChoice : sizeRow;
        sizeCol = MainMenu.colChoice > 0 ? MainMenu.colChoice : sizeCol;

        bg.gameObject.SetActive(false);
        eolText.text = null;
        eolText.gameObject.SetActive(false);
        generator = GetComponent<MazeConstructor>();
        //generator.GenerateNewMaze(13, 15);
        if(MainMenu.diff == 1)
        {
            sizeRow *= 1;
            sizeCol *= 1;
        }
        else if(MainMenu.diff == 2)
        {
            sizeRow = (int)(sizeRow * 1.3);
            sizeCol = (int)(sizeCol * 1.3);
        }
        StartNewGame();
    }

    private void StartNewGame()
    {
        timeLimit = 80;
        reduceLimitBy = 5;
        startTime = DateTime.Now;

        score = 0;
        scoreLabel.text = score.ToString();

        StartNewMaze();
    }

    private void StartNewMaze()
    {
        generator.GenerateNewMaze(sizeRow, sizeCol, OnStartTrigger, OnGoalTrigger);
        surface.BuildNavMesh();

        float x = generator.startCol * generator.hallWidth;
        float y = 0.5f;
        float z = generator.startRow * generator.hallWidth;
        player.transform.position = new Vector3(x, y, z);

        goalReached = false;
        player.enabled = true;

        // restart timer
        timeLimit -= reduceLimitBy;
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.enabled)
        {
            return;
        }

        int timeUsed = (int)(DateTime.Now - startTime).TotalSeconds;
        int timeLeft = timeLimit - timeUsed;

        scoreLabel.text = "Gold earned: " + score.ToString();

        if (timeLeft > 0)
        {
            timeLabel.text = "Time Left: " + timeLeft.ToString() + " seconds";
        }
        else
        {
            if (goalReached == true)
            {
                bg.gameObject.SetActive(true);
                eolText.text = "You Won!!!/nNow loading/nnext level";
                eolText.gameObject.SetActive(true);
                player.enabled = false;
                Invoke("StartNewGame", 3);
            }
            else
            {
                
                bg.gameObject.SetActive(true);
                timeLabel.text = "Time's up";
                player.enabled = false;
                eolText.text = "Game over!!!";
                eolText.gameObject.SetActive(true);
                Invoke("LoadSceneWithDelay", 3);
            }
        }
    }

    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Goal!");
        goalReached = true;

        score += 1;
        scoreLabel.text = "Gold earned: " + score.ToString();
        timeLimit += 5;

        Destroy(trigger);
        goalReached = false;
        generator.ReplaceTreasure(sizeRow, sizeCol, OnStartTrigger, OnGoalTrigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        if (goalReached)
        {
            Debug.Log("Finish!");
            player.enabled = false;

            Invoke("StartNewMaze", 4);
        }
    }

    private void LoadSceneWithDelay()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        SceneManager.LoadScene("IntroScreen", LoadSceneMode.Single);
    }
}
