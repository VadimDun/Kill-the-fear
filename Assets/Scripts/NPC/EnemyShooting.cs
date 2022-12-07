using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public int numOfGun = 1;

    Visibility visibility;
    public Gun gun;

    /*
    void Start()
    {
        visibility = GetComponent<Visibility>();
        gun.ChangeGun(numOfGun);
    }

    
    void Update()
    {
        if (visibility.isVisible)
        {
            Vector3 lookDirection = visibility.player.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
            gun.Shoot();
        }
    }*/
}
