using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Usa el valor de daño que desees
    public float lifeTime = 2f;
    public GameObject toxic;
    public bool playerBullet;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destruir después de un tiempo por si no choca
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerBullet)
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);

            // Efectito mágico de impacto nyaa~
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));

            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("Enemy") && playerBullet)
        {
            NormalEnemy e = collision.gameObject.GetComponent<NormalEnemy>();
            e.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("ShooterEnemy") && playerBullet)
        {
            NormalEnemyShoot s = collision.gameObject.GetComponent<NormalEnemyShoot>();
            s.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // Pew pew!! 
        }
        else if (collision.gameObject.CompareTag("Toxic") && playerBullet)
        {
            Suelo t = collision.gameObject.GetComponent<Suelo>();
            t.TakeDamage();
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
        else if (collision.gameObject.CompareTag("Floor") && !playerBullet)
        {
            Instantiate(toxic, new Vector3(transform.position.x, transform.position.y-0.2f,0), Quaternion.Euler(0,0,0));
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
        else if (collision.gameObject.CompareTag("Pasive") && playerBullet)
        {
            Amigable t = collision.gameObject.GetComponent<Amigable>();
            t.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
    }
}
