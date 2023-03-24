using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaserBeam : MonoBehaviour
{
    private PlayerGun playerGun;

    private BoxCollider2D bulletCollider;

    private Rigidbody2D bulletRigidbody;
    
    private LineRenderer lineRenderer;

    void Start()
    {
        playerGun = GetComponent<PlayerGun>();
        //Получаю префаб пули из компонента PlayerGun
        GameObject bullet = playerGun.bulletPrefab;
        //Получаю RigidBody префаба пули
        bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        //Получаю Collider префаба пули
        bulletCollider = bullet.GetComponent<BoxCollider2D>();
        //Получаю LineRender префаба пули
        lineRenderer = bullet.GetComponent<LineRenderer>();

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
