using UnityEngine;

public class Alien : MonoBehaviour
{
    public int id;
    public int speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField]
    public float timeToMove, timeToShoot;
    float counter, shootCounter;
    [SerializeField] int chanceToShoot;
    public int alienRow;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        counter = Time.fixedTime;
        shootCounter += Time.deltaTime;

        if (counter >= timeToMove)
        {
            rb.velocity = Vector2.right * speed;
        }

        if (shootCounter >= timeToShoot)
        {
            Shoot();
            shootCounter = 0;
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

        if (collision.gameObject.CompareTag("alien") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }


    public void DestroyAlien()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= chanceToShoot)
        {
        Instantiate(bulletPrefab, (transform.position + new Vector3(0, 0.5f, 0)), bulletPrefab.transform.rotation);
        }
    }
}
