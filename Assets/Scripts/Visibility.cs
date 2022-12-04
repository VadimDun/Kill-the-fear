using UnityEngine;

public class Visibility : MonoBehaviour
{
    public Rigidbody2D player;
    public bool isVisible = false;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    Rigidbody2D rb2d;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = player.position - rb2d.position;
        Vector2 spreadOnSides = new Vector2(direction.y, -direction.x).normalized * circleCollider.radius;
        LayerMask mask = LayerMask.GetMask("Visibility", "Bullets");
        if (Physics2D.Raycast(rb2d.position, direction, direction.magnitude, ~mask.value) &&
            Physics2D.Raycast(rb2d.position + spreadOnSides, direction - spreadOnSides, (direction - spreadOnSides).magnitude, ~mask.value) &&
            Physics2D.Raycast(rb2d.position - spreadOnSides, direction + spreadOnSides, (direction + spreadOnSides).magnitude, ~mask.value))                                                                                                                                                                                               
        { 
            isVisible = false;
            spriteRenderer.enabled = false;
        }
        else
        {
            isVisible = true;
            spriteRenderer.enabled = true;
        }
    }
}
