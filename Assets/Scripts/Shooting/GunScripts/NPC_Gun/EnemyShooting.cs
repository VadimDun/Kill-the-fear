using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    private EnemyGun enemyGun;
    private EnemySound enemySound;
    Visibility visibility;


    void Start()
    {
        enemySound = GetComponent<EnemySound>();    
        enemyGun = GetComponent<EnemyGun>();
        visibility = GetComponent<Visibility>();
        enemyGun.ChangeGun(enemyGun.GetNumOfGun);
        enemySound.ChangeGunSound(enemyGun.GetNumOfGun);
    }

    void Update()
    {
        if (visibility.isVisible)
        {
            Vector3 lookDirection = visibility.player.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
            enemyGun.EnemyShoot();
        }
    }
}
