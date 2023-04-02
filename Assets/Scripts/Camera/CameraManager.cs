using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // MainCamera - это единственный объект на сцене. Этот скрипт для нее
    // Она не будет уничтожаться при переходе на другую сцену

    private static CameraManager instance;

    public static CameraManager Instance => instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(this.gameObject);


    }

    public void DestroyCamera()
    {
        Destroy(this.gameObject);
    }
}
