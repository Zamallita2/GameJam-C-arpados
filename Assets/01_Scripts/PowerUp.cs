using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        ExtraLife,
        DamageBoost,
        TripleShot
    }

    public PowerUpType powerUpType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch (powerUpType)
            {
                case PowerUpType.ExtraLife:
                    player.life += 2;
                    break;
                case PowerUpType.DamageBoost:
                    player.bulletDamage = 2; 
                    break;
                case PowerUpType.TripleShot:
                    player.tripleShot = true; 
                    break;
            }

            Destroy(gameObject);
        }
    }
}
