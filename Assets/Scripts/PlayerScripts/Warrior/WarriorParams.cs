using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorParams : MonoBehaviour
{
    private int health = 100;
    [SerializeField]
    private CircleCollider2D SelfCollider; 

    public CircleCollider2D GetPlayerCollider => SelfCollider;

    private void Start()
    {
        SelfCollider= GetComponent<CircleCollider2D>();
    }

    private void Update()
    {

    }
}
