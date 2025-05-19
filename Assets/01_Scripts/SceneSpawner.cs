using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector2 spawnPosition = Vector2.zero;

    void Start()
    {
        // si no has definido un spawn, no hace nada
        if (spawnPosition == Vector2.zero) return;

        // busca al Player existente (o instáncialo si usas prefab)
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position =
                new Vector3(spawnPosition.x, spawnPosition.y, player.transform.position.z);
        }
    }
}
