using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public int id;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BoardManager.sharedInstance.ClearAdjacentAliens(this.gameObject);
        DestroyAlien();
        Destroy(collision.gameObject);
    }

    public void DestroyAlien()
    {
        Destroy(gameObject);
    }
}
