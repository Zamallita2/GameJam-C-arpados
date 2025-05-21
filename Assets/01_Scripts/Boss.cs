using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [Header("Vida del Boss")]
    public float vida = 100f; // Vida máxima uwu
    public float maxVida = 100f;

    [Header("Disparo")]
    public Transform pointA; // Extremo izquierdo del rango de disparo
    public Transform pointB; // Extremo derecho
    public GameObject bulletPrefab; // Prefab de la balita mágica
    public float bulletSpeed = 10f;
    public float tiempoDisparo = 5f;

    public GameObject enemyPrefab;

    private float tiempoRestante;

    [Header("Animaciones")]
    public Animator animator;
    public GameObject explosionBoss;

    [Header("Ojito perseguidor")]
    public GameObject ojo; // Asigna el GameObject "Ojo"
    public Transform playerTransform; // Referencia al jugador

    [Header("Audio")]
    public AudioClip fase2Sound;
    private AudioSource audioSource;
    private bool sonidoFase2Reproducido = false;



    private bool fase3Activada = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        tiempoRestante = tiempoDisparo;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        // Solo si la vida está arriba del 75% owo
        if (vida > maxVida * 0.75f)
        {
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0f)
            {
                DispararBala();
                tiempoRestante = tiempoDisparo;

                // Probabilidad del 30% de doble disparo sin cooldown uwu
                float chance = UnityEngine.Random.value; // entre 0 y 1
                if (chance <= 0.3f)
                {
                    DispararBala(); // ¡Balita extra sin esperar! 
                }
            }
        }
        else if (vida > maxVida * 0.3f)
        {
            // Reproducir sonido solo una vez al entrar en esta fase
            if (!sonidoFase2Reproducido && fase2Sound != null)
            {
                audioSource.PlayOneShot(fase2Sound);
                sonidoFase2Reproducido = true;
            }

            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0f)
            {
                DispararBala();

                // 40% chance de doble disparito
                if (UnityEngine.Random.value <= 0.4f)
                {
                    DispararBala();
                }

                // 60% chance de invocar enemigo
                if (UnityEngine.Random.value <= 0.6f && enemyPrefab != null)
                {
                    animator.SetTrigger("IsAttaking");
                    float randomX = UnityEngine.Random.Range(pointA.position.x, pointB.position.x);
                    Vector2 spawnPosition = new Vector2(randomX, -11f);
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }

                tiempoRestante = tiempoDisparo;
                ActivarOjito();
            }
        }

    }

    void ActivarOjito()
    {
        if (!fase3Activada)
        {
            GameObject nuevoOjito = Instantiate(ojo, new Vector3(-2.1f, 4.8f, 0), Quaternion.identity);

            BossEje eyeScript = nuevoOjito.GetComponent<BossEje>();
            if (eyeScript != null)
            {
                eyeScript.Activar(playerTransform); // El ojito buscará al player solito uwu
                fase3Activada = true;
            }
        }
        
    }


    void DispararBala()
    {
        animator.SetTrigger("IsAttaking");
        // Elegimos una X aleatoria entre los dos puntos uwu
        float randomX = UnityEngine.Random.Range(pointA.position.x, pointB.position.x);
        Vector2 spawnPosition = new Vector2(randomX, pointA.position.y);

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0,0,220));

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * bulletSpeed;
        }
    }

    public void TakeDamage(int amount)
    {
        vida -= amount;

        animator.SetTrigger("IsTakingDamage");

        if (vida <= 0)
        {
            Instantiate(explosionBoss, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
