using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    
    private GameObject player;

    private Player playerParams;

    private Shooting playerShooting;


    public void gameOver()
    {
        gameOverUi.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();
        
        // Выключаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = false;

        // Выключаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы игнорировать ввод у персонажа
        playerShooting.enabled = false;


    }



    public void Restart()
    {
        gameOverUi.SetActive(false);
        player.transform.position = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        //Возвращаю HP игрока в дефолтное состояния, на данный момент оно в минусе
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        // Включаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = true;

        // Включаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы больше не игнорировать ввод у персонажа
        playerShooting.enabled = true;


        // Перезагружаю сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




    public void Home(int sceneID)
    {
        gameOverUi.SetActive(false);
        SceneManager.LoadScene(sceneID);

        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        //Уничтожаю то что не уничтожается при переходе, в меню оно не нужно
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
    }
}
