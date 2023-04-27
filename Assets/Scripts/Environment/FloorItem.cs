using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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




    
    private void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.E) && OnPlayerTarget)
        {

            am.MainInventoryManager(item, itemObject);
                
        }
    }

}
