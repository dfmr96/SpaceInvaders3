using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance; //singleton

    public List<GameObject> prefabs = new List<GameObject>(); //Alien prefabs
    private GameObject[,] aliens; //matriz
    public int xSize = 11, ySize = 5; //board size
    public float paddingX = 0.05f, paddingY = 0.05f; //alien padding


    private void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        CreateInitialBoard();
    }

    private void CreateInitialBoard()
    {
        aliens = new GameObject[xSize, ySize];
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                int randomEnemy = Random.Range(0, prefabs.Capacity);
                GameObject alien = Instantiate(prefabs[randomEnemy], new Vector3(startX + (paddingX * x), startY + (paddingY * y), 0), prefabs[randomEnemy].transform.rotation);
                alien.name = string.Format("Alien[{0}][{1}]", x, y);
                alien.transform.parent = this.transform;

                aliens[x, y] = alien;
            }
        }
    }
}
