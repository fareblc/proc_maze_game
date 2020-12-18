using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject inputCol;
    public GameObject inputRow;


    static public int diff = 1; //1 for easy 2 for hard
    static public int colChoice = 0;
    static public int rowChoice = 0;

    private void Start()
    {
        if(Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void DiffEasy()
    {
        diff = 1;
    }

    public void DiffHard()
    {
        diff = 2;
    }

    public void GetRowCols()
    {
        colChoice = Convert.ToInt32(inputCol.GetComponent<Text>().text);
        rowChoice = Convert.ToInt32(inputRow.GetComponent<Text>().text);
    }


}
