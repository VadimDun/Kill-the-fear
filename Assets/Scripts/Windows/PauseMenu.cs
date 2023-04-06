using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private bool DeathWindowIsActive = false;

    public bool deathWindowIsActive
    {
        get { return DeathWindowIsActive; }
        set { DeathWindowIsActive = value; }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        CursorManager.Instance.SetMenuCursor();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
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

    private void Update()
    {
        // Если персонаж умер - тогда окно паузы нельзя вызвать
        if (DeathWindowIsActive)
            return;

        // Вызов паузы на клавишу escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
