using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private string[] scene_kill_list = { "MainMenu", "SelectLevel" };





    private void Start()
    {
        am = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }




    /*
     * ”ничтожаю все предметы, при переходе на другой уровень,
     * так как они €вл€ютс€ DontDestroyOnLoad, из-за того что стали дочерними объектами DontDestroyOnLoad
     */

    void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        if (scene_kill_list.Contains(newScene.name))
        {
            Object.Destroy(this.gameObject);
        }
    }





    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
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
