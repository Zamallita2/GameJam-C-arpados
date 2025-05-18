using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public GameObject pasiveEnemyPrefab;

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

    void Start()
    {
        currentHealth = maxHealth;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SetRandomDirection();
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
        float dirX = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // Girar según dirección del jugador
        if ((dirX > 0 && !isFacingRight) || (dirX < 0 && isFacingRight))
            Flip();

        // Saltar si están casi alineados y tocando el suelo
        if (isGrounded && Mathf.Abs(player.position.x - transform.position.x) < 1.5f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
        Debug.Log("Daño recibido. Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Murió el zombie");

            DropPowerUp();

            // Instanciar el doctor con 60% de probabilidad
            if (pasiveEnemyPrefab != null)
            {
                float chance = Random.value; // 0.0 a 1.0
                if (chance < 0.6f) // 60% de probabilidad
                {
                    Instantiate(pasiveEnemyPrefab, transform.position, Quaternion.identity);
                    FindObjectOfType<Player>()?.SumarDoctor(); //para el contador de doctores

                    ZombieCounterUI.instance?.IncrementCounter(); // << AQUI está la línea que faltaba
                    Debug.Log("Doctor instanciado (60%)");
                }
                else
                {
                    Debug.Log("No se instancia doctor (40%)");
                }
            }
            else
            {
                Debug.LogWarning("pasiveEnemyPrefab está vacío");
            }

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
