using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    
    public GameObject player;
    public void gameOver()
    {
        gameOverUi.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        gameOverUi.SetActive(false);
        player.transform.position = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);
    }
}
