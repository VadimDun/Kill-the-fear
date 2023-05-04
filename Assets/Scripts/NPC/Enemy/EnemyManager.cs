using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

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


    public void Start()
    {

        /*
         * Записываю все объекты на старте в множество 
        */


        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            FloorItem item_data = item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            AddToItemsList(item_id, item_scene);
        }

    }


    private HashSet<(int, int)> SetOfDead = new HashSet<(int, int)>();

    private HashSet<(int, int)> SetOfItems = new HashSet<(int, int)>();

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

    // Добавлен в множество предметов на уровне
    public void AddToItemsList(int id, int sceneId)
    { 
        SetOfItems.Add((id, sceneId));
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


    // Уничтожает все предметы на этаже, которых нет в списке
    // Делает неактивными тех, у которых другой индекс сцены
    // Делает активными тех, у которых индекс сцены равен индексу текущей сцены

    private int current_scene_index;
    public void KillAllNecessaryItems()
    {

        Invoke("KillItemsAfterSceneIndexLoaded", 1f);

    }






    private void KillItemsAfterSceneIndexLoaded()
    {
        current_scene_index = SceneManager.GetActiveScene().buildIndex;

        Debug.Log(current_scene_index);

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            FloorItem item_data = item.GetComponent<FloorItem>();

            int item_id = item_data.getId;

            int item_scene = item_data.GetCurrentSceneIndex;

            bool item_in_inventory = item_data.get_in_inventory_status;

            Debug.Log($"имя предмета = {item.name} сцена предмета = {item_scene}");

            if (!item_in_inventory)
            {
                if (!SetOfItems.Contains((item_id, item_scene))) { Destroy(item); }
                else if (item_scene == current_scene_index) { Debug.Log($"Предмет {item.name} был активирован"); item.SetActive(true); }
                else item.SetActive(false);
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

    public void ShowMeItems()
    {
        foreach (var elem in SetOfItems)
        {
            Debug.Log(elem);
        }
    }

    public void DestroyReaper()
    {
        Destroy(this.gameObject);
    }

}
