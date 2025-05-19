using UnityEngine;

public class SeguiminentoCamara: MonoBehaviour
{
    [Tooltip("Transform del jugador al que seguirá la cámara")]
    public Transform jugador;

    [Tooltip("Desfase opcional para centrar mejor la vista")]
    public Vector2 desplazamiento = new Vector2(0f, 1f);

    [Tooltip("Velocidad de suavizado")]
    public float suavizado = 5f;

    private void LateUpdate()
    {
        if (jugador == null) return;

        // Calcula la posición deseada manteniendo la z de la cámara
        Vector3 posicionDeseada = new Vector3(
            jugador.position.x + desplazamiento.x,
            jugador.position.y + desplazamiento.y,
            transform.position.z
        );

        // Suaviza la transición
        transform.position = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            suavizado * Time.deltaTime
        );
    }
}
