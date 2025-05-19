using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [Tooltip("Dónde debe reaparecer el jugador")]
    public Vector2 spawnPosition = new Vector2(-23.91f, -10.04f);

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No encontré al Player con tag 'Player'");
            return;
        }

        // Mensaje de debug para verificar que esto se está ejecutando
        Debug.Log($"[SceneSpawner] Recolocando Player a {spawnPosition}");

        player.transform.position = new Vector3(
            spawnPosition.x,
            spawnPosition.y,
            player.transform.position.z
        );
    }
}
