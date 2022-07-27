using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] Enemy[] prefabs; // 0-n ; 0 bottom , n top
    [SerializeField] public int killedEnemiesCount { get; private set; }
    [SerializeField] int rows = 5;
    [SerializeField] int columns = 11;
    [SerializeField] int totalEnemies => rows * columns;

    GameMenager GM;
    Player player;

    int aliveEnemiesCout => totalEnemies - killedEnemiesCount;

    float spaceBetwenEnemies = 1.25f;
    float movementSpeed = 0.5f;
    float edgeOffset = 0.9f;
    float killedEnemiesPercent;
    float repeatMovingTime = 0.5f;
    public float prepareTime = 0.025f;

    bool onceBool = false;

    List<Transform> EnemiesList = new List<Transform>();

    Vector3 movementDirection = Vector3.right;
    Vector3 leftMaxDistance;
    Vector3 rightMaxDistance;
    public Vector3 startedPosition;

    void Awake()
    {
        startedPosition = this.transform.position;
        player = FindObjectOfType<Player>();
        GM = FindObjectOfType<GameMenager>();
        leftMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.right);
        GenerateEnemies();
    }

    void Update()
    {
        if(GM.gameStarted && !onceBool)
        {
            onceBool = true;
            StartCoroutine(PrepareToBattle(0.025f));
        }
    }

    void GenerateEnemies()
    {
        float widthOfGrid = (columns - 1) * spaceBetwenEnemies;
        float heightOfGrid = (rows - 1) * spaceBetwenEnemies;

        Vector2 centerOffset = new Vector2(-widthOfGrid / 2, -heightOfGrid / 2);

        for (int row = 0; row < rows; row++)
        {
            Vector2 currentRowPosition = new Vector2(centerOffset.x, (row * spaceBetwenEnemies) + centerOffset.y);

            for (int col = 0; col < columns; col++)
            {
                Enemy enemy = Instantiate(prefabs[row], transform);
                enemy.enemyKilled += EnemyKilled;
                Vector2 position = currentRowPosition;
                position.x += col * spaceBetwenEnemies;
                enemy.transform.localPosition = position;
                EnemiesList.Add(enemy.transform);
            }
        }

        foreach (Transform enemy in EnemiesList)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public IEnumerator PrepareToBattle(float time)
    {
        List<Transform> EnemiesListCopy = new List<Transform>();

        killedEnemiesCount = 0;

        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(true);
            enemy.gameObject.SetActive(false);
            EnemiesListCopy.Add(enemy.transform);
        }

        transform.position = startedPosition;

        while (EnemiesListCopy.Count != 0)
        {
            int randomizedEnemy = Random.Range(0, EnemiesListCopy.Count);
            EnemiesListCopy[randomizedEnemy].gameObject.SetActive(true);
            EnemiesListCopy.RemoveAt(randomizedEnemy);
            yield return new WaitForSeconds(time);

            if(EnemiesListCopy.Count == 0)
            {
                foreach (Transform enemy in EnemiesList)
                {
                    enemy.gameObject.SetActive(false);
                    enemy.gameObject.SetActive(true);
                }

                AdjustEnemySpeed();
                player.enemyAreReady = true;
            }
        }

        StopAllCoroutines();
    }

    void MoveEnemies()
    {
        foreach (Transform enemy in transform)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (movementDirection == Vector3.right && enemy.position.x >= (rightMaxDistance.x - edgeOffset))
            {
                SwapDirectionAndChangeRow();
                return;
            }
            else if (movementDirection == Vector3.left && enemy.position.x <= (leftMaxDistance.x + edgeOffset))
            {
                SwapDirectionAndChangeRow();
                return;
            }
        }

        transform.position += movementDirection * movementSpeed;
    }

    void SwapDirectionAndChangeRow()
    {
        movementDirection = -movementDirection;

        Vector3 tempPosition = transform.position;
        tempPosition.y -= 1.0f;
        transform.position = tempPosition;
    }

    void EnemyKilled()
    {
        killedEnemiesCount++;
        CheckPercentOfKilledEnemies();
        CheckWinLoose();
    }

    void CheckPercentOfKilledEnemies()
    {
        killedEnemiesPercent = ((float)killedEnemiesCount / (float)totalEnemies) * 100;

        if (this.gameObject.transform.childCount - killedEnemiesCount == 1)
        {
            repeatMovingTime = 0.025f;
            AdjustEnemySpeed();
        }
        else if (killedEnemiesPercent >= 80)
        {
            repeatMovingTime = 0.1f;
            AdjustEnemySpeed();
        }
        else if (killedEnemiesPercent >= 50)
        {
            repeatMovingTime = 0.25f;
            AdjustEnemySpeed();
        }
        else if (killedEnemiesPercent >= 20)
        {
            repeatMovingTime = 0.4f;
            AdjustEnemySpeed();
        }
    }

    void AdjustEnemySpeed()
    {
        CancelInvoke();
        InvokeRepeating("MoveEnemies", 0.1f, repeatMovingTime);
    }

    void CheckWinLoose()
    {
        if(killedEnemiesCount >= totalEnemies)
        {
            CancelInvoke();
            transform.position = startedPosition;
            GM.Win();
            repeatMovingTime = 0.5f;
        }
    }

    public void CancelAllInvoke()
    {
        CancelInvoke();
    }

}
