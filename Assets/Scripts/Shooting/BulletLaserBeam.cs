using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaserBeam : MonoBehaviour
{
    private Collider2D bulletCollider;

    private Rigidbody2D bulletRigidbody;
    
    private LineRenderer lineRenderer;

    void Start()
    {
        //Получаю RigidBody
        bulletRigidbody = GetComponentInChildren<Rigidbody2D>();
        //Получаю Collider
        bulletCollider = GetComponentInChildren<Collider2D>();
        //Получаю LineRender
        lineRenderer = GetComponentInChildren<LineRenderer>();

    }


    private void FixedUpdate()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = bulletRigidbody.position;

        positions[1] = bulletRigidbody.position + (Vector2)bulletRigidbody.transform.right * bulletCollider.bounds.size.x;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);

    }




}
