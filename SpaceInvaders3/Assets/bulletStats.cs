using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class bulletStats : MonoBehaviour
{
    [SerializeField] int BulletSpeed;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = Vector2.up * BulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boundary") || collision.gameObject.CompareTag("barrier"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
        Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

    }

}
