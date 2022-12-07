using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    //Дальномер с позиции игрока
    [SerializeField]
    private RangeFinder rangeFinder;
    //Точка ведения огня (на стволе)
    [SerializeField]
    private Transform firePoint;
    //Выбранная пуля (Тут должна быть PlayerBullet)
    [SerializeField]
    private GameObject bulletPrefab;

    //Коррекция для дробовика (связано с LookDirection спрайта)
    private WarriorMovement correction;

    //Игрок и скрипт пули
    private GameObject player;
    private PlayerBullet bullet;

    //Минимальная дистанция для стрельбы
    private float MinFireDist = 0.35f;



    public void PlayerShoot() => Shoot(bullet, bulletPrefab, firePoint);

    //Состояние курка
    private bool isTriggerPulled = false;
    public bool GetIsTriggered() => isTriggerPulled;

    public void PullTheTrigger()
    {
        isTriggerPulled = !isTriggerPulled;
    }

    protected override void Shoot(Bullet bullet, GameObject bulletPrefab, Transform firePoint)
    {
        if (Time.time - lastShotTime < delayBetweenShots) { return; }
        lastShotTime = Time.time;
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<PlayerBullet>();
        bullet.damage = damage;
        bullet.bulletSpeed = bulletSpeed;
        switch (current_gun)
        {
            case Guns.shotgun:
                Vector2 direction = transform.right;
                float normalAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (360f - correction.angleDifference);
                for (int i = -4; i < 4; ++i)
                {
                    if (i != 0)
                    {
                        float angle = normalAngle + pelletsSpread * i + Random.Range(-pelletsDeviation, pelletsDeviation);
                        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle, Vector3.forward)).GetComponent<PlayerBullet>();
                        bullet.damage = damage;
                        bullet.bulletSpeed = bulletSpeed;
                    }
                }
                break;
        }
    }



    void Start()
    {
        correction = GetComponent<WarriorMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        bullet = player.GetComponent<PlayerBullet>();
    }
}
