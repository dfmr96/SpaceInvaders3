using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstance; //singleton

    public List<GameObject> prefabs = new List<GameObject>(); //Alien prefabs
    [SerializeField] GameObject player, barrier;
    [SerializeField] List<GameObject> barriers;
    public GameObject[,] aliens; //matriz
    public int xSize = 11, ySize = 5; //board size
    public float paddingX = 0.05f, paddingY = 0.05f; //alien padding

    public float counter;
    float timeReducer = 0.3f;

    public int totalEnemies = 0;
    public int score;
    public int highScore;
    int chainEnemies;


    public int lives;

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
        Time.timeScale = 1;
        barriers = new List<GameObject>();
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore", highScore);
        }
        CreateInitialBoard();
        CreateBarriers();
        Instantiate(player, player.transform.position, player.transform.rotation);
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (totalEnemies == 0)
        {
            CreateInitialBoard();
            CreateBarriers();
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
        chainEnemies++;
        Vector2Int alienShootPosition = FindAlien(alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x, alienShootPosition.y + 1), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x, alienShootPosition.y - 1), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x + 1, alienShootPosition.y), alien);
        KillAdjacentAlien(new Vector2Int(alienShootPosition.x - 1, alienShootPosition.y), alien);

        UpdateScore();
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
            chainEnemies++;
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
    }

    public void GameOver()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        UIManager.sharedInstance.ShowGameOverScreen();
        Time.timeScale = 0;
    }

    void UpdateScore()
    {
        if (chainEnemies > 1)
        {
            score += chainEnemies * Fibonacci()[chainEnemies + 1] * 10;
        }
        else
        {
            score += 10;
        }
        chainEnemies = 0;
    }

    public int[] Fibonacci()
    {
        int i;
        int[] fibNumbers = new int[6];
        fibNumbers[0] = 0;
        fibNumbers[1] = 1;
        for (i = 2; i < 6; i++)
        {
            fibNumbers[i] = fibNumbers[i - 1] + fibNumbers[i - 2];

        }
        return fibNumbers;
    }

    public IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(player, player.transform.position, player.transform.rotation);
        StopCoroutine(SpawnPlayer());
    }

    public void SpawnPlayerMethod()
    {
        StartCoroutine(SpawnPlayer());
    }

    public void CreateBarriers()
    {
        foreach (GameObject barrier in barriers)
        {
            Destroy(barrier);
        }

        for (int i = 0; i < 4; i++)
        {
            var barrierGO = Instantiate(barrier, (new Vector3(-9.55f + (6 * i), -4, 0)), barrier.transform.rotation);
            barriers.Add(barrierGO);
        }
    }
}