using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Bullet : MonoBehaviour
{
 
    public BoxCollider2D collider;

    private RaycastHit2D HitTheWall;

    private float DeathTime;


    Rigidbody2D rb2d;

    private GameObject Warrior;
    private Gun UserGun;
    private WarriorMovement correction;

    private const float LifeTime = 2;
    private float LifeTimer = 0;


    public float bulletSpeed = 10f;
    public int damage = 10;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(bulletSpeed * rb2d.transform.right.x, bulletSpeed * rb2d.transform.right.y);
        Warrior = GameObject.FindWithTag("Player");
        UserGun = Warrior.GetComponent<Gun>();
        correction = Warrior.GetComponent<WarriorMovement>();
    }

    private void Update()
    {

        LifeTimer += Time.deltaTime;
        if (LifeTimer >= LifeTime) { Destroy(gameObject); LifeTimer = 0; }

        float CastAngle = Mathf.Atan2(rb2d.transform.right.x, rb2d.transform.right.y) * Mathf.Rad2Deg + correction.WRC;

        HitTheWall = Physics2D.Raycast(rb2d.position, rb2d.transform.right, collider.size.x, LayerMask.GetMask("Bullet", "Creatures"));

        float DistNum = HitTheWall.distance * Time.deltaTime;
        DeathTime = Time.fixedDeltaTime + DistNum;



        if (HitTheWall)
        {

            if (UserGun.GetDistToTarget < 0.3f) { Destroy(gameObject); }
            else { Destroy(gameObject, DeathTime); }


        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null) { enemy.TakeDamage(damage); Destroy(gameObject, DeathTime); }
    }




}
