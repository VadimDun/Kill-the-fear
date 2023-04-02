using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelsScript : MonoBehaviour
{
    public void Level1()
    {
        CursorManager.Instance.SetScopeCursor();
        SceneManager.LoadScene(2);
    }

    public void Level2() 
    {
        CursorManager.Instance.SetScopeCursor();
        SceneManager.LoadScene(3);
    }

    public void Level3()
    {
        CursorManager.Instance.SetScopeCursor();
        SceneManager.LoadScene(4);
    }

    public void Level4()
    {
        CursorManager.Instance.SetScopeCursor();
        SceneManager.LoadScene(5);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
