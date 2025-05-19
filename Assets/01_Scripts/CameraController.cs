using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float minX, maxX, minY, maxY;

    void LateUpdate()
    {
        Vector3 pos = player.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
