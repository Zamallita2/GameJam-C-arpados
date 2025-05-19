using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { ExtraLife, DamageBoost, TripleShot }
    public PowerUpType powerUpType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<Player>();
        if (player == null) return;

        // 1) Aplica el efecto local
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

        // 2) ¡Guarda en GameManager al instante!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerStats(
                player.Contador,
                player.life,
                player.bulletDamage,
                player.tripleShot
            );
            Debug.Log($"[PowerUp] Guardado → Contador={player.Contador}, life={player.life}, dmg={player.bulletDamage}, 3xShot={player.tripleShot}");
        }

        Destroy(gameObject);
    }
}
