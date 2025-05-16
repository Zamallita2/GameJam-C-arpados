using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suelo : MonoBehaviour
{
    public int damageAmount = 1; // Puedes cambiarlo desde el inspector owo

    public int GetDamage()
    {
        return damageAmount;
    }
}
