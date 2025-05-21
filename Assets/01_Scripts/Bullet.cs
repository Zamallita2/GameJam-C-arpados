using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Usa el valor de daño que desees
    public float lifeTime = 6f;
    public GameObject toxic;
    public bool playerBullet;

    public int contador=0;
    public GameObject explosionProjectile;
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destruir después de un tiempo por si no choca
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerBullet)
        {
            Player p = collision.gameObject.GetComponent<Player>();
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            p.TakeDamage(damage);

            // Efectito mágico de impacto nyaa~
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));

            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("Enemy") && playerBullet)
        {
            NormalEnemy e = collision.gameObject.GetComponent<NormalEnemy>();
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            e.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("ShooterEnemy") && playerBullet)
        {
            NormalEnemyShoot s = collision.gameObject.GetComponent<NormalEnemyShoot>();
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            s.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // Pew pew!! 
        }
        else if (collision.gameObject.CompareTag("Toxic") && playerBullet)
        {
            Suelo t = collision.gameObject.GetComponent<Suelo>();
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            t.TakeDamage();
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
        else if (collision.gameObject.CompareTag("Floor") && !playerBullet)
        {
            Instantiate(toxic, new Vector3(transform.position.x, transform.position.y-0.2f,0), Quaternion.Euler(0,0,0));
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
        else if (collision.gameObject.CompareTag("Plataform") && !playerBullet)
        {
            float chance = Random.value;
            if (chance <= 0.3f)
            {
                Instantiate(toxic, new Vector3(transform.position.x, transform.position.y - 0.2f, 0), Quaternion.Euler(0, 0, 0));
                Instantiate(explosionProjectile, transform.position, Quaternion.identity);
                //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject); // ¡Splaaash del moco verde! 
            }
        }
        else if (collision.gameObject.CompareTag("Pasive") && playerBullet)
        {
            Amigable t = collision.gameObject.GetComponent<Amigable>();
            Instantiate(explosionProjectile, transform.position, Quaternion.identity);
            t.TakeDamage(damage);
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
        else if (collision.gameObject.CompareTag("Boss") && playerBullet)
        {
            Boss t = collision.gameObject.GetComponent<Boss>();
            if (contador > 0)
            {
                Instantiate(explosionProjectile, transform.position, Quaternion.identity);
                t.TakeDamage(damage);
            }
            else
            {
                Instantiate(explosionProjectile, transform.position, Quaternion.identity);
                t.TakeDamage(0);
                
            }
            //Instantiate(effect, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject); // ¡Splaaash del moco verde! 
        }
    }
}
