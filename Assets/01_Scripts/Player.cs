// Player.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Contador")]
    public int Contador = 0;

    [Header("Movimiento y Salto")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Vida e Invulnerabilidad")]
    public int life = 3;
    public float invulnerabilityDuration = 3f;
    private bool isInvulnerable = false;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int bulletDamage = 1;
    public bool tripleShot = false;
    public Transform firePoint;
    private string lastRot = "D";

    [Header("Teletransporte")]
    [Tooltip("Nombre exacto de la escena que quieres cargar (sin .unity)")]
    public string nextSceneName;

    // Componentes internos
    private Rigidbody2D rb;
    private SpriteRenderer[] sprites;
    private Animator animator;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Carga los stats y power-ups guardados
        if (GameManager.Instance != null)
        {
            Contador = GameManager.Instance.Contador;
            life = GameManager.Instance.life;
            bulletDamage = GameManager.Instance.bulletDamage;
            tripleShot = GameManager.Instance.tripleShot;
            Debug.Log($"[Player] Stats cargados → Contador={Contador}, life={life}, dmg={bulletDamage}, 3xShot={tripleShot}");
        }
    }

    void Update()
    {
        // Comprueba si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("isJumping");
        }
        // Movimiento derecha
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.identity;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            firePoint.rotation = Quaternion.Euler(0, 0, 0);
            firePoint.localPosition = new Vector2(1.44f, 0f);
            lastRot = "D";
        }
        // Movimiento izquierda
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            firePoint.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.localPosition = new Vector2(1.44f, 0f);
            lastRot = "I";
        }
        // Apuntar arriba
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            firePoint.rotation = Quaternion.Euler(0, 0, 90);
            firePoint.localPosition = new Vector2(0f, 1.7f);
        }
        // Apuntar abajo
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            firePoint.rotation = Quaternion.Euler(0, 0, -90);
            firePoint.localPosition = new Vector2(0f, -1.4f);
        }
        // Orientación por defecto
        else
        {
            if (lastRot == "D")
            {
                transform.rotation = Quaternion.identity;
                firePoint.rotation = Quaternion.Euler(0, 0, 0);
                firePoint.localPosition = new Vector2(1.44f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                firePoint.rotation = Quaternion.Euler(0, 180, 0);
                firePoint.localPosition = new Vector2(1.44f, 0f);
            }
        }

        // Actualiza animaciones
        animator.SetBool("isWalking",
            Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A));
        animator.SetBool("isLookingUp",
            Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
        animator.SetBool("isLookingDown",
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));

        // Disparo
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        if (tripleShot)
        {
            for (int i = -1; i <= 1; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = (Quaternion.Euler(0, 0, i * 15) * firePoint.right).normalized;
                rbBullet.velocity = dir * bulletSpeed;
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            rbBullet.velocity = firePoint.right * bulletSpeed;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        life -= amount;
        animator.SetTrigger("isHit");
        if (life <= 0)
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(InvulnerabilityTimer());
    }

    IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        float blinkInterval = 0.2f;

        while (elapsed < invulnerabilityDuration)
        {
            foreach (var sr in sprites) sr.enabled = false;
            yield return new WaitForSeconds(blinkInterval / 2);
            foreach (var sr in sprites) sr.enabled = true;
            yield return new WaitForSeconds(blinkInterval / 2);
            elapsed += blinkInterval;
        }

        isInvulnerable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Portal: guarda stats/power-ups y cambia de escena
        if (other.CompareTag("Portal"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SavePlayerStats(
                    Contador,
                    life,
                    bulletDamage,
                    tripleShot
                );

            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Suelo tóxico
        if (other.CompareTag("Toxic"))
        {
            Suelo toxic = other.GetComponent<Suelo>();
            if (toxic != null)
                TakeDamage(toxic.GetDamage());
        }
    }

    public void SumarDoctor()
    {
        Contador++;
        Debug.Log($"[Player] SumarDoctor → Contador ahora {Contador}");
    }
}
