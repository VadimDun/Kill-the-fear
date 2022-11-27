using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarriorMovement : MonoBehaviour
{
    //140 градусов на доворот
    private const float WarriorRotateCorrection = 140f;

    //Компоненты персонажа
    [SerializeField]
    private float WarriorSpeed = 1.0f;
    [SerializeField]
    private Rigidbody2D Warrior;
    [SerializeField]
    private Camera cam;

    //Векторы
    Vector2 MovementDirection;
    Vector2 MousePosition;

    void Update()
    {
        //Направлкние движения
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");
        //Позиция курсора
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        //Поворот персонажа
        Vector2 LookDirection = MousePosition - Warrior.position;
        Warrior.rotation = (Mathf.Atan2(LookDirection.x, LookDirection.y) * -Mathf.Rad2Deg) + WarriorRotateCorrection;
        Warrior.velocity = MovementDirection * WarriorSpeed;

    }
}
