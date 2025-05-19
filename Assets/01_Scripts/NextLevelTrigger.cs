using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena a cargar")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        SceneManager.LoadScene(nextSceneName);
    }
}
