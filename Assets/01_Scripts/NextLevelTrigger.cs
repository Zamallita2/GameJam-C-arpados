using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena a cargar (sin .unity)")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Guarda de nuevo, por seguridad
        var p = other.GetComponent<Player>();
        if (p != null && GameManager.Instance != null)
            GameManager.Instance.SavePlayerStats(
                p.Contador,
                p.life,
                p.bulletDamage,
                p.tripleShot
            );

        SceneManager.LoadScene(nextSceneName);
    }
}
