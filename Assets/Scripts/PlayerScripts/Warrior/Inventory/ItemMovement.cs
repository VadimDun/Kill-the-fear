using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private Transform parentTransform;

    private GameObject gunSlot;

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
    }

    public void OnDrag(PointerEventData eventData)
    {

        // Обновляю позицию картинки на Canvas
        Vector2 newPosition = eventData.position - offset;
        (transform as RectTransform).anchoredPosition = newPosition;

        gunSlot = null;

        // Пытаюсь получить объект картинки слота
        if (eventData.pointerCurrentRaycast.gameObject.tag == "GunSlot")
        {
            gunSlot = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (gunSlot != null)
        { 
            /*

            //Получаю оружие, которое передаю
            GameObject gun = transform.GetChild(0).gameObject;

            // Получаю слот, в который хочу передать оружие
            AmmunitionGunSlot slot = gunSlot.GetComponent<AmmunitionGunSlot>();

            bool SuccessAddition;

            // Передаю оружие в новый слот
            GameObject.Find("Main Camera").GetComponent<AmmunitionManager>().PutWeaponToSlot(gun, slot, out SuccessAddition);

            */
        }

        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия
        transform.SetParent(parentTransform);
    }
}