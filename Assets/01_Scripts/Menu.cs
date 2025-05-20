using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public enum TipoBoton { JUGAR, SALIR }
    public TipoBoton tipo;

    void OnMouseDown()
    {
        switch (tipo)
        {
            case TipoBoton.JUGAR:
                SceneManager.LoadScene("SampleScene");
                break;

            case TipoBoton.SALIR:
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Para detener el juego en el editor
#endif
                break;
        }
    }
}
