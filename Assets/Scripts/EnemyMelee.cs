using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int damage = 1;

    private Transform player;
    private float cooldownTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move toward the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Attack if cooldown is done
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                MeleeAttack();
                cooldownTimer = attackCooldown;
            }
        }
    }

    void MeleeAttack()
    {
        Debug.Log("Melee attack triggered!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
