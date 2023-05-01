using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorItem : MonoBehaviour
{
    [SerializeField] private Item item;

    public Item getItem => item;

    [SerializeField] private GameObject itemObject;

    public GameObject GetItemObject => itemObject;


    private bool OnPlayerTarget = false;

    private InventoryManager am;





    private void Start()
    {
        am = GameObject.Find("Main Camera").GetComponent<InventoryManager>();
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        { 
            OnPlayerTarget = true;
            
        }

    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().CompareTag("Player"))
        {
           OnPlayerTarget = false;
        }
    }











    private const float AddingCooldown = 0.1f;


    // Разрешает подбирать предметы по истечению определенного времени
    IEnumerator StartAllowAdding() 
    {
        yield return new WaitForSeconds(AddingCooldown);
        FloorItem.is_added_now = false;
    }









    public static bool is_added_now = false;
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && OnPlayerTarget)
        {
            if (!is_added_now) { am.MainInventoryManager(item, itemObject); is_added_now = true; StartCoroutine(StartAllowAdding()); }
                
        }
    }

}
