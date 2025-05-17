using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Usa el valor de daño que desees
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destruir después de un tiempo por si no choca
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponentInParent<NormalEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        var shooter = other.GetComponentInParent<NormalEnemyShoot>();
        if (shooter != null)
        {
            shooter.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }




}
