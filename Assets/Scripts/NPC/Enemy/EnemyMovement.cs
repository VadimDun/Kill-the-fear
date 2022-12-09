using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Разница в LookDir ствола и спрайта     
    private float AngleDifference;
    public float angleDifference => AngleDifference;

    //LookDirection спрайта
    private float StartEnemyDir;
    //LookDirection ствола                            
    private float StartGunDir;

    //Ось ствола           
    [SerializeField]
    private Transform EnemyGunAxis;

    //Ось спрайта         
    [SerializeField]
    private Transform EnemyAxis;

    void Start()
    {
        StartEnemyDir = EnemyAxis.rotation.z * Mathf.Rad2Deg;
        StartGunDir = EnemyGunAxis.rotation.z * Mathf.Rad2Deg;
        AngleDifference = StartEnemyDir + StartGunDir;
    }

}

