using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amigable : MonoBehaviour
{
    public int life = 20;
    public GameObject pasiveEnemyPrefab;
    public float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isRunning = false;
    private float idleTimer = 0f;
    private float idleDuration = 0f;
    private int idleDirection = 1;
    private bool isFacingRight = true;

    private float escapeTimer = 0f;
    private float escapeDuration = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SetRandomDirection();
    }

    void Update()
    {
        if (isRunning)
        {
            Patrol();
            escapeTimer += Time.deltaTime;
            if (escapeTimer >= escapeDuration)
            {
                Destroy(gameObject); // desaparece si sobrevive 20 segs owo
            }
        }
    }

    public void TakeDamage(int amount)
    {
        life -= amount;

        if (!isRunning)
        {
            isRunning = true;
            if (animator != null)
                animator.SetBool("IsRunning", true); // activa animación uwu
        }

        if (life <= 0)
        {
            float chance = Random.value; // 0.0 a 1.0
            if (chance < 1f) // 100% (ajusta si quieres menos, como 0.6f)
            {
                Instantiate(pasiveEnemyPrefab, transform.position, Quaternion.identity);
                FindObjectOfType<Player>()?.SumarDoctor(); // para el contador de doctores
            }
            Destroy(gameObject); // muere >:3
        }
    }

    void Patrol()
    {
        idleTimer += Time.deltaTime;
        rb.velocity = new Vector2(idleDirection * moveSpeed, rb.velocity.y);

        if ((idleDirection > 0 && !isFacingRight) || (idleDirection < 0 && isFacingRight))
            Flip();

        if (idleTimer >= idleDuration)
            SetRandomDirection();
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
}
