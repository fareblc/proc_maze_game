using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;

    public void KillPlayer()
    {
        this.GetComponent<GameController>().bg.gameObject.SetActive(true);
        this.GetComponent<GameController>().eolText.gameObject.SetActive(true);
        this.GetComponent<GameController>().eolText.text = "Game Over";
        this.GetComponent<GameController>().player.enabled = false;
        Invoke("KillPlayerWithDelay", 3);
    }


    private void KillPlayerWithDelay()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("IntroScreen", LoadSceneMode.Single);
    }
}
