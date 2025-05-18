using TMPro;
using UnityEngine;

public class ZombieCounterUI : MonoBehaviour
{
    public static ZombieCounterUI instance;

    public TextMeshProUGUI counterText;
    private int zombieCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void IncrementCounter()
    {
        zombieCount++;
        counterText.text = "Zombies desinfectados: " + zombieCount;
    }
}
