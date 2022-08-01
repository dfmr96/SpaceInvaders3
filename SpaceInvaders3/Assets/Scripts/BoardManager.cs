using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance; //singleton

    public List<GameObject> prefabs = new List<GameObject>(); //Alien prefabs
    public GameObject[,] aliens; //matriz
    public int xSize = 11, ySize = 5; //board size
    public float paddingX = 0.05f, paddingY = 0.05f; //alien padding

    public float counter;
    float timeReducer = 0.3f;

    public int totalEnemies = 0;
    public int score;
    public int highScore;


    public int lives;

    [SerializeField] float fallCounter;
    public bool leftBoundaryCanTranslate, rightBoundaryCanTranslate;

    private void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        CreateInitialBoard();
    }

    private void Update()
    {
        counter += Time.deltaTime;
        fallCounter += Time.deltaTime;
        if (totalEnemies == 0)
        {
            CreateInitialBoard();
        }
    }

    private void CreateInitialBoard()
    {
        counter = 0;
        aliens = new GameObject[xSize, ySize];
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                totalEnemies++;
                int randomEnemy = Random.Range(0, prefabs.Capacity);
                GameObject alien = Instantiate(prefabs[randomEnemy], new Vector3(startX + (paddingX * x), startY + (paddingY * y), 0), prefabs[randomEnemy].transform.rotation);
                alien.name = string.Format("Alien[{0}][{1}]", x, y);
                alien.transform.parent = this.transform;
                aliens[x, y] = alien;
                alien.GetComponent<Alien>().alienRow = FindAlien(alien).y;
                alien.GetComponent<Alien>().timeToMove = FindAlien(alien).y * timeReducer;
            }
        }
    }
    public Vector2Int FindAlien(GameObject alien)
    {

        int w = aliens.GetLength(0);
        int h = aliens.GetLength(1);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (aliens[i, j] == alien)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public void ClearAdjacentAliens(GameObject alien)
    {
        Vector2Int alienShootPosition = FindAlien(alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x, alienShootPosition.y + 1), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x, alienShootPosition.y - 1), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x + 1, alienShootPosition.y), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x - 1, alienShootPosition.y), alien);

    }

    public void KillAdjacentAlien(Vector2Int position, GameObject alien)
    {
        bool isInMatrix = position.x >= 0 && position.y >= 0 && position.x < aliens.GetLength(0) && position.y < aliens.GetLength(1);
        if (!isInMatrix)
        {
            return;
        }

        GameObject adjacentObject = aliens[position.x, position.y];

        if (adjacentObject == null)
        {
            return;
        }
        Alien adjacent = adjacentObject.GetComponent<Alien>();
        if (adjacent != null && adjacent.id == alien.GetComponent<Alien>().id)
        {
            adjacent.DestroyAlien();
        }
    }

    public void ChangeDirection(int row)
    {
        int w = aliens.GetLength(0);

        for (int i = 0; i < w; i++)
        {
            if (aliens[i, row] != null)
            {
                aliens[i, row].GetComponent<Alien>().speed *= -1;
            }
        }
    }
    public void TranslateEnemiesDown()
    {



        foreach (GameObject alien in aliens)
        {
            if (alien != null)
            {
                alien.transform.Translate(Vector3.down);
            }
        }
        fallCounter = 0;


    }
}