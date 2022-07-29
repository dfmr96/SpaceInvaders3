using UnityEngine;

public class Alien : MonoBehaviour
{
    public int id;
    public int speed;
    [SerializeField]
    public float timeToMove;
    float counter;
    public int alienRow;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        counter = Time.fixedTime;

        if (counter >= timeToMove)
        {
            rb.velocity = Vector2.right * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("boundary"))
        {
            BoardManager.sharedInstance.ChangeDirection(alienRow);
        }

        if (collision.gameObject.CompareTag("bullet"))
        {
            BoardManager.sharedInstance.ClearAdjacentAliens(this.gameObject);
            DestroyAlien();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("alien"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }


    public void DestroyAlien()
    {
        Destroy(gameObject);
    }
}
