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
    }

    public void Restart()
    {
        player.transform.position = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
