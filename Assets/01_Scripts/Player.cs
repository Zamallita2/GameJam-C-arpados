using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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



    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform firePoint;
    private Rigidbody2D rb;
    private SpriteRenderer[] sprites;
    private Animator animator;



    private Vector2 lastHorizontalDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("isJumping");
        }

        Vector2 inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputDirection = new Vector2(0, 4);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            inputDirection = new Vector2(0, -2);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection = new Vector2(2, 0);
            lastHorizontalDirection = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection = new Vector2(-2, 0);
            lastHorizontalDirection = Vector2.left;
        }

        // Movimiento horizontal
        if (inputDirection.x != 0)
        {
            float moveX = Mathf.Sign(inputDirection.x) * moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        }

        if (lastHorizontalDirection == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        else if (lastHorizontalDirection == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 180, 0); 


        // Firepoint dirección
        Vector2 firePointDirection;

        if (inputDirection.y != 0)
        {
            firePointDirection = new Vector2(0, inputDirection.y);
        }
        else if (inputDirection.x != 0)
        {
            firePointDirection = new Vector2(inputDirection.x, 0);
        }
        else
        {
            firePointDirection = lastHorizontalDirection * 2;
        }

        
        firePoint.localPosition = new Vector3(firePointDirection.x, firePointDirection.y, firePoint.localPosition.z);

        
        Vector2 realDirection = firePointDirection.normalized;

        
        if (lastHorizontalDirection == Vector2.left)
            realDirection.x *= -1;

        float angle = Mathf.Atan2(realDirection.y, realDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);


        // ANIMACIONES
        animator.SetBool("isWalking", inputDirection.x != 0);
        animator.SetBool("isLookingUp", Input.GetKey(KeyCode.UpArrow));
        animator.SetBool("isLookingDown", Input.GetKey(KeyCode.DownArrow));

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
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = (Quaternion.Euler(0, 0, i * 15) * firePoint.right).normalized;
                rb.velocity = dir * bulletSpeed;

                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = firePoint.right * bulletSpeed;

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }





    void TakeDamage(int amount)
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
}
