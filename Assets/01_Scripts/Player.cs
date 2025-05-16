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

    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform firePoint; 
    private Rigidbody2D rb;
    private SpriteRenderer[] sprites;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
    private Vector2 lastHorizontalDirection = Vector2.right; // Por defecto mirando a la derecha owo

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

        // Mover solo si hay input horizontal
        if (inputDirection.x != 0)
        {
            float moveX = Mathf.Sign(inputDirection.x) * moveSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, 0, 0));
        }

        // Flip del player con escala
        if (lastHorizontalDirection == Vector2.right)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (lastHorizontalDirection == Vector2.left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Firepoint direction owo
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

        // Aplicar la posición local del firePoint con flip incluido
        float flipX = Mathf.Sign(transform.localScale.x); // 1 o -1 dependiendo de si está volteado
        firePoint.localPosition = new Vector3(firePointDirection.x * flipX, firePointDirection.y, firePoint.localPosition.z);

        //  ¡Aquí la magia kawaii del ángulo correcto, usando la posición REAL del firePoint!
        Vector2 realDirection = firePoint.localPosition.normalized;

        //  Detectamos si el player está volteado
        if (transform.localScale.x < 0)
        {
            realDirection.y *= -1; // Invertimos X si está volteado, para que el ángulo sea correcto uwu
        }

        float angle = Mathf.Atan2(realDirection.y, realDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

    }


    void TakeDamage(int amount)
    {
        if (isInvulnerable) return; // uwu no le haces daño si está invulnerable

        life -= amount;

        if (life <= 0)
        {
            Destroy(gameObject); // Adiós, gatit@ hermoso ;w;
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
            // ¡Desactiva los sprites para el parpadeo~!
            foreach (var sr in sprites)
                sr.enabled = false;

            yield return new WaitForSeconds(blinkInterval / 2);

            foreach (var sr in sprites)
                sr.enabled = true;

            yield return new WaitForSeconds(blinkInterval / 2);

            elapsed += blinkInterval;
        }

        // Asegura que queden visibles
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
