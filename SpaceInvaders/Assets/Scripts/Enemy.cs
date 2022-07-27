using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public Sprite[] animationSprites;
    [SerializeField] public float animationSpeed;
    [SerializeField] public int killingPrice;
    [SerializeField] public float missileAttackRate;

    GameMenager WLS;
    SpriteRenderer spriteRenderer;
    JSONReader jsonReader;
    PauseMenuManager paused;
    public Projectile rocketPrefab;
    public System.Action enemyKilled;

    int currentAnimationFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        WLS = FindObjectOfType<GameMenager>();
        paused = FindObjectOfType<PauseMenuManager>();
        jsonReader = FindObjectOfType<JSONReader>();
        SetStats();
        InvokeRepeating(nameof(SpriteAnimation), this.animationSpeed, this.animationSpeed);
        InvokeRepeating("RocketShoot", missileAttackRate, missileAttackRate);
    }

    void SpriteAnimation()
    {
        if (animationSprites.Length <=1) { return; }

        currentAnimationFrame++;

        if (currentAnimationFrame >= this.animationSprites.Length)
        {
            currentAnimationFrame = 0;
        }

        spriteRenderer.sprite = this.animationSprites[currentAnimationFrame];
    }

    void RocketShoot()
    {
        if (!this.gameObject.activeInHierarchy) { return; }

        if (Random.value >= missileAttackRate)
        {
            Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        }
    }

    void SetStats()
    {
        for(int i=0; i<jsonReader.enemyStats.listOfStatsForEnemy.Length; i++)
        {
            if(jsonReader.enemyStats.listOfStatsForEnemy[i].enemyName == transform.tag)
            {
                this.killingPrice = jsonReader.enemyStats.listOfStatsForEnemy[i].killingPrice;
                this.missileAttackRate =  jsonReader.enemyStats.listOfStatsForEnemy[i].missileAttackRate;
            }
        }
    }

    public void CancelAllInvoke()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            WLS.SetScore(killingPrice);
            enemyKilled.Invoke();
            gameObject.SetActive(false);
        }
    }
}
