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
        Debug.Log("Input go!");
        gameOverUi.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        gameOverUi.SetActive(false);
        player.transform.position = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;
        player.GetComponent<Player>().playerHealth = 100;
        player.GetComponent<Player>().playerIsDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //”станавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        //”ничтожаю то что не уничтожаетс€ при переходе, в меню оно не нужно
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
