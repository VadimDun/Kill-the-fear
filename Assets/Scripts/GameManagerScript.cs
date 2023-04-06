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

    private PauseMenu pauseMenu;

    private int StartSceneIndex;

    private Vector3 SpawnPointPosition;

    private EnemyManager enemyReaper;

    private GameObject levelChanger;


    public void gameOver()
    {
        gameOverUi.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();
        pauseMenu = GetComponent<PauseMenu>();
        enemyReaper = GameObject.Find("EnemyReaper").GetComponent<EnemyManager>();

        // Выключаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = false;

        // Выключаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы игнорировать ввод у персонажа
        playerShooting.enabled = false;

        // Выключаю стрельбу всем террористам
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = false;
            }
        }

        // Отключаю ввод для паузы
        pauseMenu.deathWindowIsActive = true;

        Debug.Log($"Start index = {StartSceneIndex}");

    }


    private void Start()
    {
        // Получаю сцену, с которой мы изначально загрузились
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Получаю позицию точки спавна игрока
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // Получаю скипт анимаций перехода
        levelChanger = GameObject.Find("LevelChanger");
    }



    public void Restart()
    {

        // Перезагружаю сцену
        SceneManager.LoadSceneAsync(StartSceneIndex);

        //Включаю затемнение
        levelChanger.SetActive(false);

        gameOverUi.SetActive(false);
        player.transform.position = SpawnPointPosition;

        //Возвращаю HP игрока в дефолтное состояния, на данный момент оно в минусе
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        // Включаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = true;

        // Включаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы больше не игнорировать ввод у персонажа
        playerShooting.enabled = true;

        // Включаю стрельбу всем террористам
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyShooting enemyShooting = enemy.GetComponentInChildren<EnemyShooting>();
            if (enemyShooting != null)
            {
                enemyShooting.enabled = true;
            }
        }

        // Разрешаю ввод для паузы
        pauseMenu.deathWindowIsActive = false;

        // Отчищаю список убитых у жнеца
        enemyReaper.SetOfDeadEdit.Clear();

        // Выключаю затемнение
        levelChanger.SetActive(true);

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
