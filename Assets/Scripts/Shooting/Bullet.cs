using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class Bullet : MonoBehaviour
{
 
    public BoxCollider2D collider;
    private GameObject Terrorist;

    private RaycastHit2D HitTheWall;
    private RaycastHit2D HitEnemy;




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
        Terrorist = GameObject.FindGameObjectWithTag("Enemy");
        correction = Warrior.GetComponent<WarriorMovement>();
    }

    private void Update()
    {

        LifeTimer += Time.deltaTime;
        if (LifeTimer >= LifeTime) { Destroy(gameObject); LifeTimer = 0; }

        float CastAngle = Mathf.Atan2(rb2d.transform.right.x, rb2d.transform.right.y) * Mathf.Rad2Deg + correction.WRC;

        //Баг тут

        HitTheWall = Physics2D.Raycast(rb2d.position, rb2d.transform.right, collider.size.x, LayerMask.GetMask("Bullet", "Creatures"));

        HitEnemy = Physics2D.Raycast(rb2d.position, rb2d.transform.right, collider.size.x, LayerMask.GetMask("Bullet", "Enemy"));

        //Скорее всего это решается через BoxCast, так как только в нем можно корректировать угол.
        //HitEnemy = Physics2D.BoxCast(rb2d.position, collider.size , CastAngle, rb2d.transform.right, collider.size.x * 2, LayerMask.GetMask("Bullet", "Enemy"));


        float DistNum = HitTheWall.distance * Time.deltaTime;

        
        if (HitTheWall)
        {

            if (UserGun.GetDistToTarget < 0.3f) { Destroy(gameObject); }
            else { Destroy(gameObject, Time.fixedDeltaTime + DistNum); }


        }

        if (HitEnemy)
        { 
            Enemy enemy = Terrorist.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log($"Its here");
        }
    }



    
}
