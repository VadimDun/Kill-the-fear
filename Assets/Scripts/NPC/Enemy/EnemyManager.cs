using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Единственный объект на карте, который отслеживает и убивает убитых 

    private static EnemyManager instance;

    public static EnemyManager Instance => instance;

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

    private HashSet<(int, int)> SetOfDead = new HashSet<(int, int)>();

    public HashSet<(int, int)> SetOfDeadEdit
    {
        get { return SetOfDead; }
        set { SetOfDead = value; }
    }


    // Добавляем в мертвых
    public void AddToDeadList(int id, int sceneId)
    {
        SetOfDead.Add((id, sceneId));
    }


    // Отправляем прямиком в ад
    public void ToHell()
    {
        foreach (var elem in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy enemy = elem.GetComponent<Enemy>();
            if (SetOfDead.Contains((enemy.GetId, enemy.GetSceneId)))
            {
                Destroy(elem);
            }
        }
    }

    // Для дебага
    public void ShowMeTheDead()
    {
        foreach (var elem in SetOfDead)
        {
            Debug.Log(elem);
        }
    }

    public void DestroyReaper()
    {
        Destroy(this.gameObject);
    }

}
