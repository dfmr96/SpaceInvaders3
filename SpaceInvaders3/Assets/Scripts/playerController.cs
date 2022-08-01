using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] int speed = 5;
    private const string horizontal = "Horizontal";
    Vector2 movementDirection;

    [SerializeField] AudioSource shootSound, explosionSound;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float rateOfFire;
    [SerializeField] float rateOfFireTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw(horizontal)) > 0f) //GetAxisRaw for values of -1 (down), 0 or 1 ( up) in Unity Input System; Mathf.Abs to return |-1| values and active condition.
        {
            MovePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero; //if player dont use movement buttons, gameobject will stop
        }

        rateOfFireTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && rateOfFireTimer < 0)
        {
            Shoot();
            shootSound.Play();
            rateOfFireTimer = rateOfFire;
        }
    }

    public void MovePlayer()
    {
        movementDirection = new Vector2(Input.GetAxisRaw(horizontal), 0); // Catch vector2 movement direction
        rb.velocity = movementDirection.normalized * speed; //normalize to get movement direction and increase the vector magnitude with * speed
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, (transform.position + new Vector3(0, 0.5f, 0)), bulletPrefab.transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            BoardManager.sharedInstance.lives--;

            if (BoardManager.sharedInstance.lives > 0)
            {
                DestroyPlayer();
            }
            else
            {

                BoardManager.sharedInstance.GameOver();
            }
        }
    }

    void DestroyPlayer()
    {
        anim.SetBool("isDestroyed", true);
        explosionSound.Play();
        Destroy(gameObject, 1f);
        BoardManager.sharedInstance.SpawnPlayerMethod();
    }
}
