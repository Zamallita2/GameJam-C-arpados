using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float jumpForce = 6f;
    public int maxHealth = 3;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform player;
    public Rigidbody2D rb;
    public GameObject[] powerUpPrefabs;

    private int currentHealth;
    private bool isGrounded;
    private bool isFacingRight = true;

    private float idleTimer = 0f;
    private float idleDuration = 0f;
    private int idleDirection = 1; // -1 izquierda, 1 derecha
    

    private Animator Anim;
    public GameObject explosionEnemy;


    void Start()
    {
        currentHealth = maxHealth;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SetRandomDirection();
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void MoveTowardsPlayer()
    {
        float distanceX = player.position.x - transform.position.x;

        // No moverse si están demasiado cerca
        if (Mathf.Abs(distanceX) > 0.1f)
        {
            float dirX = Mathf.Sign(distanceX);
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            Anim.SetBool("IsWalking", true);

            // Girar según dirección del jugador
            if ((dirX > 0 && !isFacingRight) || (dirX < 0 && isFacingRight))
                Flip();
        }
        else
        {
            // Detenerse si está muy cerca
            rb.velocity = new Vector2(0, rb.velocity.y);
            Anim.SetBool("IsWalking", false);
        }

        // Saltar si están alineados y tocando el suelo
        if (isGrounded && Mathf.Abs(distanceX) < 1.5f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Anim.SetTrigger("IsJumping");
        }
    }


    void Patrol()
    {
        idleTimer += Time.deltaTime;
        rb.velocity = new Vector2(idleDirection * moveSpeed, rb.velocity.y);

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


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Instantiate(explosionEnemy, transform.position, Quaternion.identity);
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

        }
    }
}
