using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] public Vector3 projectileDirection;
    [SerializeField] GameObject blastPrefab;

    public System.Action objectDestroyed;

    void Update()
    {
        MoveProjectile();
    }

    void MoveProjectile()
    {
        transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(objectDestroyed != null)
        {
            objectDestroyed.Invoke();
        }
        Destroy(this.gameObject);

        GameObject blastPrefab = Instantiate(this.blastPrefab, transform.position, Quaternion.identity);
        Destroy(blastPrefab, 0.25f);
    }
}
