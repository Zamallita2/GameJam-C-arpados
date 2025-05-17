using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float detectionRange = 40f;
    public float jumpForce = 6f;
    public int maxHealth = 3;
    private int currentHealth;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private GameObject player;
    private bool isGrounded;
    private bool isFacingRight = true;

    public GameObject[] powerUpPrefabs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        Vector2 direction = (player.transform.position - transform.position).normalized;

        if (distanceToPlayer <= detectionRange)
        {
            rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);

            if (isGrounded && Mathf.Abs(player.transform.position.x - transform.position.x) < 1.5f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
                Flip();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); 
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            DropPowerUp();
            Destroy(gameObject);
        }
    }

    void DropPowerUp()
    {
        if (powerUpPrefabs.Length > 0)
        {
            int index = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[index], transform.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.SendMessage("TakeDamage", 1);
            }

            Destroy(gameObject);
        }
    }
}
