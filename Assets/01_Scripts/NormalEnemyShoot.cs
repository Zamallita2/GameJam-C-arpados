using UnityEngine;

public class NormalEnemyShoot : MonoBehaviour
{

    public GameObject pasiveEnemyPrefab;


    public float shootCooldown = 2f;
    public float visionRange = 6f;
    public float bulletSpeed = 30f;
    public float moveSpeed = 2f;
    public float life=3;
    public Transform player;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public GameObject[] powerUps;
    public Rigidbody2D rb;
    private Animator Anim;

    private float lastShootTime;
    private bool isFacingRight = true;

    private float idleTimer = 0f;
    private float idleDuration = 0f;
    private int idleDirection = 1; // -1 izquierda, 1 derecha

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        SetRandomDirection();

        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= visionRange)
        {
            LookAtPlayer();

            // Disparar si pasa el cooldown
            if (Time.time > lastShootTime + shootCooldown)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
        else
        {
            Patrol();
        }
    }

    void LookAtPlayer()
    {
        float dirX = player.position.x - transform.position.x;

        if ((dirX > 0 && !isFacingRight) || (dirX < 0 && isFacingRight))
            Flip();

        Anim.SetBool("IsWalking", false);
    }

    void Patrol()
    {
        idleTimer += Time.deltaTime;
        rb.velocity = new Vector2(idleDirection * moveSpeed, rb.velocity.y);
        Anim.SetBool("IsWalking", true);

        if ((idleDirection > 0 && !isFacingRight) || (idleDirection < 0 && isFacingRight))
            Flip();

        if (idleTimer >= idleDuration)
        {
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        idleDirection = Random.value > 0.5f ? 1 : -1;
        idleDuration = Random.Range(2f, 5f);
        idleTimer = 0f;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Anim.SetTrigger("IsShooting");
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null && player != null)
        {
            Vector2 dir = (player.position - shootPoint.position).normalized;
            rb.velocity = dir * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (powerUps.Length > 0)
        {
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], transform.position, Quaternion.identity);
        }

        // Instanciar el doctor (pasive)
        if (pasiveEnemyPrefab != null)
        {
            Instantiate(pasiveEnemyPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

}
