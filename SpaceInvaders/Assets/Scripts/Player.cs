using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerHealth = 2;
    [SerializeField] float playerSpeed = 5.5f;
    [SerializeField] Projectile bulletPrefab;

    GameMenager WLS;

    bool bulletExist = false;
    public bool enemyAreReady = false;

    Vector3 leftMaxDistance;
    Vector3 rightMaxDistance;

    void Start()
    {
        WLS = FindObjectOfType<GameMenager>();
        WLS.SetLives(playerHealth);
        leftMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.zero) + new Vector3(0.5f, 0f, 0f);
        rightMaxDistance = Camera.main.ViewportToWorldPoint(Vector3.right) - new Vector3(0.5f, 0f, 0f);
    }

    void Update()
    {
        if(WLS.gameStarted)
        {
            PlayerMovement();
            ShootBullet();
        }
    }

    void PlayerMovement()
    {
        if(Input.GetKey(KeyCode.RightArrow) && transform.position.x < rightMaxDistance.x)
        {
            transform.position += Vector3.right * playerSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > leftMaxDistance.x)
        {
            transform.position += Vector3.left * playerSpeed * Time.deltaTime;
        }
    }

    void ShootBullet()
    {
        if(Input.GetKeyDown(KeyCode.Space) && enemyAreReady)
        {
            if (bulletExist) { return; }

            Projectile bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.objectDestroyed += BulletAreDestroyed;
            bulletExist = true;
        }
    }

    void BulletAreDestroyed()
    {
        bulletExist = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerHealth--;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            playerHealth = 0;
        }

        WLS.SetLives(playerHealth);

        if(playerHealth <= 0)
        {
            WLS.Loose();
        }
    }
}
