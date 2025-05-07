using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 2f; // bullets per second
    public float detectionRange = 5f;
    public LayerMask enemyLayer;
    public Transform firePoint;

    private float fireCooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown -= Time.deltaTime;

        Collider2D closestEnemy = FindClosestEnemyInRange();
        if (closestEnemy != null)
        {
            Debug.Log("Enemy found: " + closestEnemy.name);

            if (fireCooldown <= 0f)
            {
                Debug.Log("Shooting at: " + closestEnemy.name);
                ShootAt(closestEnemy.transform.position);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    Collider2D FindClosestEnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        Debug.Log("Enemies found: " + hits.Length);
        Collider2D closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit;
            }
        }

        return closest;
    }

    void ShootAt(Vector2 targetPos)
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Missing bulletPrefab or firePoint!");
            return;
        }

        Vector2 direction = (targetPos - (Vector2)firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * 30f; // bullet speed
        }
        else
        {
            Debug.LogWarning("Bullet prefab missing Rigidbody2D!");
        }

        Debug.Log("Bullet fired in direction: " + direction);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
