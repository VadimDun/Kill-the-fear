using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private Transform parentTransform;

    private GameObject gunSlot;

    private bool flag_on_start = true;


    public void OnBeginDrag(PointerEventData eventData)
    {

        // Запоминаю Transform родительского объекта
        parentTransform = transform.parent;

        // Устанавливаю родительский объект в качестве инвентаря
        transform.SetParent(GameObject.Find("Inventory").transform);

        // Рассчитываю смещение между позицией картинки и позицией мыши
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );


        

        // Ставлю индикаторы на передний план

        foreach (GameObject indicator in GameObject.FindGameObjectsWithTag("Indicator"))
        {

            if (indicator != null)
            {
                // Перед тем как установить родителя в качестве инвентаря - запоминаем исходного родителя
                Indicator ind = indicator.GetComponent<Indicator>();
                if (ind != null)
                {
                    if (flag_on_start)
                    {
                        // Перед тем как установить родителя в качестве инвентаря - запоминаем исходного родителя
                        ind.RememperParent(indicator.transform.parent.gameObject);
                    }

                    // Устанавливаем родителя в качестве инвентаря, чтобы поместить индикатор на передний план
                    indicator.transform.SetParent(GameObject.Find("Inventory").transform);
                }

            }


        }
    }









    public void OnDrag(PointerEventData eventData)
    {



        // Обновляю позицию картинки на Canvas
        Vector2 newPosition = eventData.position - offset;
        (transform as RectTransform).anchoredPosition = newPosition;

        gunSlot = null;


        // Пытаюсь получить объект картинки слота
        if (eventData.pointerCurrentRaycast.gameObject.tag == "Indicator")
        {
            Indicator ind = eventData.pointerCurrentRaycast.gameObject.GetComponent<Indicator>();
            if (ind != null)
            {
                gunSlot = ind.GetIndicatorParent;
            }

        }
    }










    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject[] indicators = GameObject.FindGameObjectsWithTag("Indicator").ToArray();

        for (int i = 0; i < indicators.Count(); i++)
        {

            Indicator ind = indicators[i].GetComponent<Indicator>();

            if (ind != null)
            {
                if (indicators[i] != null)
                {
                    // Устанавливаю родителя
                    indicators[i].transform.SetParent(GameObject.Find($"GunSlot({i + 1})").transform);
                    
                    // Устанавливаю индикатор на нужный мне индекс 
                    indicators[i].transform.SetSiblingIndex(1);

                    // Получаю родителя
                    GameObject ind_parent = indicators[i].transform.parent.gameObject;


                    // Запоминаю родителя
                    ind.RememperParent(ind_parent);


                    // Устанавливаю индикатор на позицию родительского слота
                    indicators[i].transform.position = ind_parent.GetComponent<AmmunitionGunSlot>().SlotDefaultPosition;

                    // Последующие разы в начале не будем запоминать родительский объект за ненадобностью
                    flag_on_start = false;
                }
            }


        }

        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия (для объекта, который перетаскиваем)
        transform.SetParent(parentTransform);

        if (gunSlot != null)
        {

            // Получаю оружие, которое передаю
            GameObject gun = transform.GetChild(0).gameObject;

            // Получаю предмет, который передаю 
            Item item = gun.GetComponent<FloorItem>().getItem;

            // Получаю слот, в который хочу передать оружие
            AmmunitionGunSlot slot = gunSlot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            GameObject.Find("Main Camera").GetComponent<AmmunitionManager>().PutWeaponToSlot(item, gun, slot, out SuccessAddition);

            if (!SuccessAddition)
            { 
                // Логика если оружие не добавилось в слот 
            }

            
        }


    }
}