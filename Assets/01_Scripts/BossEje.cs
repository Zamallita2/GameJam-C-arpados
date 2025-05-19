using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEje : MonoBehaviour
{
    public Transform objetivo; // El player uwu
    public float velocidad = 5f;
    public int daño = 10;

    private bool activo = false;

    public void Activar(Transform target)
    {
        objetivo = target;
        activo = true;
    }

    void Update()
    {
        if (!activo || objetivo == null) return;

        // Movimiento hacia el player uwu
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate que tu player tenga el tag "Player"
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.TakeDamage(daño);
        }
    }
}
