using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Inscribed")]
    public int maxHealth;
    public float maxSpeed;
    public float freeRoamSpeed;

    [Header("Dynamic")]
    public int health;
    public float speed = 30;
    public float[] position = new float[3];
    public int initiativeBonus = 0;
    public float remainingSpeed;

    [Header("Attributes")]
    public int strength { get; private set; }
    public int influence { get; private set; } //charisma
    public int reflex { get; private set; } //dexterity
    public int intinuity { get; private set; } //wisdom
    public int intelligence { get; private set; }
    public int constitution { get; private set; }
    public int proficiencyBonus { get; private set; }


    private void Awake()
    {
        speed = maxSpeed;
        remainingSpeed = speed;
        health = maxHealth;
    }

    public int GetStrengthMod()
    {
        int temp = strength;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }

    public int GetInfluenceMod()
    {
        int temp = influence;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }

    public int GetReflexMod()
    {
        int temp = reflex;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }

    public int GetIntinuityMod()
    {
        int temp = intinuity;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }

    public int GetIntelligenceMod()
    {
        int temp = intelligence;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }

    public int GetConstitutionMod()
    {
        int temp = constitution;

        if (temp % 2 != 0) temp--;

        if (temp > 10) return (temp - 10) / 2;
        else if (temp < 10) return -1 * ((temp - 10) / 2);
        else return 0;
    }
}
