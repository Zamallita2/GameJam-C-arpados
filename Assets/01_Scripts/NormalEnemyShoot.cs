using UnityEngine;

public class NormalEnemyShoot : MonoBehaviour
{
    public float shootCooldown = 2f;
    public float visionRange = 6f;
    public float bulletSpeed = 5f;
    public Transform player;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject[] powerUps;
    private float lastShootTime;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= visionRange && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (player.position - shootPoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        Die();
    }

    void Die()
    {
        if (powerUps.Length > 0)
        {
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
