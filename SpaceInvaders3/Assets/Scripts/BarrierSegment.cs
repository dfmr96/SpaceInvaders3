using UnityEngine;

public class BarrierSegment : MonoBehaviour
{
    [SerializeField] int health = 3;
    [SerializeField] Sprite[] segments;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            health--;

            if (health > 0)
            {
                GetComponent<SpriteRenderer>().sprite = segments[health - 1];
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if(collision.gameObject.CompareTag("alien"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
