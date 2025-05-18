using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int Contador = 0;



    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int life = 3;
    public float invulnerabilityDuration = 3f;
    private bool isInvulnerable = false;
    private bool isGrounded;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int bulletDamage = 1;
    public bool tripleShot = false;
    private string lastRot="D";


    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform firePoint;
    private Rigidbody2D rb;
    private SpriteRenderer[] sprites;
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verificamos si el michi está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);



        // Animación de salto 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("isJumping"); // ¡Saltito kawaii! 
        }



        // Movimiento a la derecha 
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);



            firePoint.rotation = Quaternion.Euler(0, 0, 0);
            firePoint.localPosition = new Vector2(1.44f, 0f);



            lastRot = "D";
        }
        // Movimiento a la izquierda 
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);



            firePoint.rotation = Quaternion.Euler(0, 180, 0);
            firePoint.localPosition = new Vector2(1.44f, 0f);



            lastRot = "I";
        }



        // Mirar arriba 
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            firePoint.rotation = Quaternion.Euler(0, 0, 90);
            firePoint.localPosition = new Vector2(0f, 1.7f);
        }



        // Mirar abajo 
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            firePoint.rotation = Quaternion.Euler(0, 0, -90);
            firePoint.localPosition = new Vector2(0f, -1.4f);
        }



        // Posición default cuando no se presiona nada 
        else
        {
            if (lastRot == "D")
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                firePoint.rotation = Quaternion.Euler(0, 0, 0);
                firePoint.localPosition = new Vector2(1.44f, 0f);
            }
            else if (lastRot == "I")
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                firePoint.rotation = Quaternion.Euler(0, 180, 0);
                firePoint.localPosition = new Vector2(1.44f, 0f);
            }
        }



        // Animacioncitas tiernas uwu 
        animator.SetBool("isWalking", Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A));
        animator.SetBool("isLookingUp", Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
        animator.SetBool("isLookingDown", Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));



        // Pew pew nyaa~
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (tripleShot)
        {
            for (int i = -1; i <= 1; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = (Quaternion.Euler(0, 0, i * 15) * firePoint.right).normalized;
                rb.velocity = dir * bulletSpeed;

                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;

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
            foreach (var sr in sprites)
                sr.enabled = false;

            yield return new WaitForSeconds(blinkInterval / 2);

            foreach (var sr in sprites)
                sr.enabled = true;

            yield return new WaitForSeconds(blinkInterval / 2);

            elapsed += blinkInterval;
        }

        foreach (var sr in sprites)
            sr.enabled = true;

        isInvulnerable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Toxic"))
        {
            Suelo toxic = other.GetComponent<Suelo>();
            if (toxic != null)
            {
                TakeDamage(toxic.GetDamage());
            }
        }
    }

    public void SumarDoctor()
    {
        Contador++;
        Debug.Log("Zombies desinfectados por el jugador: " + Contador);
    }


}
