using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    
    private GameObject player;

    private GameObject Face_UI;

    private Player playerParams;

    private Shooting playerShooting;

    private PauseMenu pauseMenu;

    private InventoryMenu inventoryMenu;

    private InventoryManager inventoryManager;

    private int StartSceneIndex;

    private Vector3 SpawnPointPosition;

    private EnemyManager enemyReaper;

    private CanvasTransition transition;

    private Gun gun;


    public void gameOver()
    {
        // Если игрок умер - закрываю окно паузы 
        if (!pauseMenu.PauseWindowIsNotActive)
        {
            pauseMenu.Resume();
        }


        // Если игрок умер - закрываю окно инвентаря
        if (!inventoryMenu.inventoryWindowIsNotActive)
        {
            inventoryMenu.InventoryClose();
        }

        // Выключаю лицевой UI
        Face_UI.SetActive(false);


        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();

        gameOverUi.SetActive(true);

        // Замораживаю игрока
        FreezePlayer();

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

        // Отключаю ввод для инвентаря
        inventoryMenu.deathWindowIsActive = true;

        

    }


    public void FreezePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerParams = player.GetComponent<Player>();
        playerShooting = player.GetComponent<Shooting>();

        enemyReaper = GameObject.Find("EnemyReaper").GetComponent<EnemyManager>();
        gun = player.GetComponent<PlayerGun>();

        // Выключаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = false;

        // Выключаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Если во время окна смерти (или чего другого) курок в зажатом положении - я его отключаю
        gun.TriggerIsPulled = false;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы игнорировать ввод у персонажа
        playerShooting.enabled = false;
    }

    public void UnfreezePlayer()
    {
        // Включаю передвижение, поворот
        player.GetComponent<WarriorMovement>().enabled = true;

        // Включаю физику
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Передаю состояние окна в скрипт игрока Shooting, чтобы больше не игнорировать ввод у персонажа
        playerShooting.enabled = true;
    }


    private void Start()
    {

        pauseMenu = GetComponent<PauseMenu>();

        inventoryMenu = GetComponent<InventoryMenu>();

        // Получаю сцену, с которой мы изначально загрузились
        StartSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Получаю позицию точки спавна игрока
        SpawnPointPosition = GameObject.Find("PlayerSpawnPoint").GetComponent<Transform>().position;

        // Получаю скипт анимаций перехода
        transition = GameObject.Find("LevelChanger").GetComponent<CanvasTransition>();

        Face_UI = GameObject.Find("FaceUI");

        inventoryManager = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }



    public void Restart()
    {

        //Устанавливаю курсор
        CursorManager.Instance.SetScopeCursor();

        // Перезагружаю сцену
        SceneManager.LoadScene(StartSceneIndex);

        // Перезагружаю инвентарь
        inventoryManager.ResetInventory();

        //Включаю затемнение
        transition.StartDeathTransition();

        gameOverUi.SetActive(false);

        // Возвращаю игрока на стартовую позицию
        player.transform.position = SpawnPointPosition;

        //Возвращаю HP игрока в дефолтное состояния, на данный момент оно в минусе
        playerParams.playerHealth = playerParams.GetDefaultHP;
        playerParams.playerIsDead = false;

        UnfreezePlayer();

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

        // Разрешаю ввод для инвентаря
        inventoryMenu.deathWindowIsActive = false;

        // Включаю лицевой UI
        Face_UI.SetActive(true);

        // Отчищаю список убитых у жнеца
        enemyReaper.SetOfDeadEdit.Clear();

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
