using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject GetPauseMenu => pauseMenu;

    private InventoryMenu inventoryMenu;

    private GameManagerScript gameManagerScript;


    private bool DeathWindowIsActive = false;

    private bool pauseWindowIsNotActive = true;

    public bool PauseWindowIsNotActive => pauseWindowIsNotActive;




    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    private bool InventoryWindowIsActive = false;

    public bool inventoryWindowIsActive
    {
        get { return InventoryWindowIsActive; }
        set { InventoryWindowIsActive = value; }
    }

    public void Pause()
    {
        pauseWindowIsNotActive = !pauseWindowIsNotActive;
        if (pauseWindowIsNotActive)
            Resume();
        else
        {
            // Замораживаю игрока
            gameManagerScript.FreezePlayer();

            pauseMenu.SetActive(true);
            
            // Выключаю ввод инвентарю
            inventoryMenu.pauseWindowIsActive = true;

            CursorManager.Instance.SetMenuCursor();
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        // Размораживаю игрока
        gameManagerScript.UnfreezePlayer();

        pauseWindowIsNotActive = true;
        pauseMenu.SetActive(false);

        // Включаю ввод инвентарю
        inventoryMenu.pauseWindowIsActive = false;

        CursorManager.Instance.SetScopeCursor();
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        //Устанавливаю курсор
        CursorManager.Instance.SetMenuCursor();
        
        //Уничтожаю то что не уничтожается при переходе, в меню оно не нужно
        PlayerManager.Instance.DestroyPlayer();
        CameraManager.Instance.DestroyCamera();
        CanvasManager.Instance.DestroyCanvas();
        EnemyManager.Instance.DestroyReaper();
        PauseManager.Instance.DestroyPause();
        EventManager.Instance.DestroyEventSys();
        SceneManager.LoadScene(sceneID);
    }


    private void Start()
    {
        inventoryMenu = GetComponent<InventoryMenu>();

        gameManagerScript = GetComponent<GameManagerScript>();
    }


    private void Update()
    {
        // Если персонаж умер - тогда окно паузы нельзя вызвать
        if (DeathWindowIsActive || InventoryWindowIsActive)
            return;

        // Вызов паузы на клавишу escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        Debug.Log("Окно инвентаря открыто? = " + InventoryWindowIsActive);

    }
}
