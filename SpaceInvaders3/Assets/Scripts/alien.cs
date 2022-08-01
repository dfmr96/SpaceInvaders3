using UnityEngine;

public class Alien : MonoBehaviour
{
    public int id;
    public float speed;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField]
    public float timeToMove, timeToShoot;
    float counter, shootCounter;
    [SerializeField] int chanceToShoot;
    public int alienRow;
    Rigidbody2D rb;
    Animator anim;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        shootCounter += Time.deltaTime;

        if (BoardManager.sharedInstance.counter >= timeToMove)
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
            if (collision.gameObject.name == "Left" && BoardManager.sharedInstance.leftBoundaryCanTranslate == true)
            {
                BoardManager.sharedInstance.TranslateEnemiesDown();
                BoardManager.sharedInstance.leftBoundaryCanTranslate = false;
                BoardManager.sharedInstance.rightBoundaryCanTranslate = true;
            } 

            if (collision.gameObject.name == "Right" && BoardManager.sharedInstance.rightBoundaryCanTranslate == true)
            {
                BoardManager.sharedInstance.TranslateEnemiesDown();
                BoardManager.sharedInstance.rightBoundaryCanTranslate = false;
                BoardManager.sharedInstance.leftBoundaryCanTranslate = true;
            }

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
        Destroy(gameObject, 0.5f);
        anim.SetBool("isDead", true);
        BoardManager.sharedInstance.totalEnemies--;
    }

    void Shoot()
    {
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= chanceToShoot)
        {
            Instantiate(bulletPrefab, (transform.position + new Vector3(0, 0.25f, 0)), bulletPrefab.transform.rotation);
        }
    }
}
