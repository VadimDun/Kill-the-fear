using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 offset; // смещение между позицией картинки и позицией мыши

    private Transform parentTransform;

    private GameObject gunImage;

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

        gunImage = null;

        // Пытаюсь получить объект картинки слота
        if (eventData.pointerCurrentRaycast.gameObject.tag == "GunImage")
        {
            gunImage = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (gunImage != null)
        { 
            // Передаю оружие в новый слот

        }

        // Устанавливаю в качестве родительского объекта тот, который был родительским до нажатия
        transform.SetParent(parentTransform);
    }
}