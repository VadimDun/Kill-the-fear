using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorItem : MonoBehaviour
{
    [SerializeField] private Item item;

    private GameObject GrabImage;

    public Item getItem => item;

    [SerializeField] private GameObject itemObject;

    public GameObject GetItemObject => itemObject;


    private bool OnPlayerTarget = false;

    private InventoryManager am;

    private Vector3 offset = new Vector3 (0.1f, 0.1f, 0);



    private void Start()
    {
        am = GameObject.Find("Main Camera").GetComponent<InventoryManager>();

        GrabImage = GameObject.Find("Main Camera").GetComponent<GameManagerScript>().GetE_image;

    }





    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") { Destroy(transform.gameObject); }
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }







    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            OnPlayerTarget = true;
        }

    }









    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().CompareTag("Player"))
        {
            // Даю кнопке новую позицию рядом с предметом, и делаю её активной
            SetGrabImage();
            GrabImage.SetActive(true);

        }
    }








    private void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.GetComponent<Collider2D>().CompareTag("Player"))
        {
            // Делаю кнопку неактивной
            GrabImage.SetActive(false);

            OnPlayerTarget = false;
        }
    }









    // Подбирает тот объект, на котором активна кнопка (спрайт) E
    private static Item target_item;

    private static GameObject target_object;
    private void SetGrabImage()
    {
        GrabImage.transform.position = transform.position + offset;

        target_item = item;

        target_object = itemObject;

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
            if (!is_added_now) { is_added_now = true; am.MainInventoryManager(target_item, target_object); StartCoroutine(StartAllowAdding()); }
                
        }
    }

}
