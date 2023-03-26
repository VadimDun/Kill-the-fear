using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    //Получаю огневую точку с необходимым компонентом transform  (currentPoint)
    private FirePoint firePoints;

    private GameObject firePoint;

    private Transform firePointAxis;

    [SerializeField]
    RangeFinder rf;


    [SerializeField]
    LineRenderer lineRenderer;


    private void Start()
    {
        firePoints = GetComponent<FirePoint>();
        firePoint = firePoints.GetCurrentPoint;
        firePointAxis = firePoint.transform;
    }


    private void FixedUpdate()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = firePointAxis.position;
        positions[1] = rf.GetHit.point;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
        Debug.Log(rf.GetDistToTarget);

    }
}

