using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class mag : MonoBehaviour
{
    // Емкость магазина
    private int capacity;

    // Текущее количество патрон в магазине
    private int current_bullet_count;

    // Стек патронов
    public Stack<GameObject> bullets;

    // Префаб пули 
    [SerializeField] private GameObject bulletPrefab;




    public int get_current_bullet_count => current_bullet_count;

    public int get_capacity => capacity;



    private void Start()
    {
        // Получаю емкость магазина
        Mag mag_item = GetComponent<FloorItem>().getItem as Mag;
        capacity = mag_item.GetCapacity;

        // Инициализирую стек
        bullets = new Stack<GameObject>();

        // Заполняю обойму пулями 
        if (bulletPrefab != null)
        {
            for (int i = 0; i < capacity; i++)
            {
                GameObject bulletInstance = Instantiate(bulletPrefab, transform);
                bulletInstance.SetActive(false);
                bullets.Push(bulletInstance);
                
            }


        }


        // Инициализирую текущее количество патрон
        current_bullet_count = bullets.Count;
    }


    public GameObject TakeBullet()
    {
        if (current_bullet_count > 0)
        {
            GameObject takenBullet = bullets.Pop();

            current_bullet_count--;

            return takenBullet;
        }
        else 
        {
            
            return null;
        }
    }


    public void LoadBullet(GameObject bullet, out GameObject load_result)
    {
        // Если добавление пройдет не успешно - на выход возвращаем передаваемую пулю
        load_result = bullet;

        if (current_bullet_count < capacity)
        {
            bullets.Push(bullet);

            // Если добавление прошло успешно, то подаем на выход null
            load_result = null;
        }
    }




}
