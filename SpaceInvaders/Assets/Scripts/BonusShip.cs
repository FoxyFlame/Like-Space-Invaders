using UnityEngine;

public class BonusShip : MonoBehaviour
{
    [SerializeField] int maxPriceForKill;
    [SerializeField] int minPriceForKill;

    float movementSpeed = 0.25f;

    GameMenager GM;
    JSONReader jsonReader;

    Vector3 leftMaxDistance;
    Vector3 rightMaxDistance;
    Vector3 movementDirection = Vector3.right;

    void Start()
    {
        GM = FindObjectOfType<GameMenager>();
        jsonReader = FindObjectOfType<JSONReader>();
        SetStats();
        leftMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.zero) - new Vector3(50f,0,0);
        rightMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.right) + new Vector3(50f, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int bonusPrice = Random.Range(minPriceForKill/10, (maxPriceForKill/10)+1);
        bonusPrice *= 10;

        GM.SetScore(bonusPrice);
        RestartShip();
    }

    void MoveShip()
    {
        if (GM.gameStarted)
        {
            if (!transform.gameObject.activeInHierarchy) { return; }

            if (movementDirection == Vector3.right && transform.position.x >= rightMaxDistance.x)
            {
                movementDirection = -movementDirection;
            }
            else if (movementDirection == Vector3.left && transform.position.x <= leftMaxDistance.x)
            {
                movementDirection = -movementDirection;
            }

            transform.position += movementDirection * movementSpeed;
        }  
    }

    public void RestartShip()
    {
        Vector3 tempPosition = transform.position;
        tempPosition.x = leftMaxDistance.x;
        transform.position = tempPosition;
        CancelInvoke("MoveShip");
        InvokeRepeating("MoveShip", 0.1f, 0.05f);
    }

    void SetStats()
    {
        for (int i = 0; i < jsonReader.enemyStats.listOfStatsForEnemy.Length; i++)
        {
            if (jsonReader.enemyStats.listOfStatsForEnemy[i].enemyName == transform.tag)
            {
                this.minPriceForKill = jsonReader.enemyStats.listOfStatsForEnemy[i].killingPriceMin;
                this.maxPriceForKill = jsonReader.enemyStats.listOfStatsForEnemy[i].killingPriceMax;
            }
        }
    }
}
