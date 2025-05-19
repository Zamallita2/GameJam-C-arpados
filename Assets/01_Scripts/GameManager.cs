// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Stats del jugador")]
    public int Contador;
    public int life;
    public int bulletDamage;
    public bool tripleShot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Guarda únicamente los stats y power-ups que realmente usas.
    /// </summary>
    public void SavePlayerStats(int contador, int life, int bulletDamage, bool tripleShot)
    {
        Contador = contador;
        this.life = life;
        this.bulletDamage = bulletDamage;
        this.tripleShot = tripleShot;
    }
}
