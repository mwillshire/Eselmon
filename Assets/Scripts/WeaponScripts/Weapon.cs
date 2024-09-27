//This script will go on all weapon prefabs

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum eWeaponType { strength, reflex };

    [Header("Inscribed")]
    public int diceType;
    public eWeaponType weaponType;
    public float weaponRange;

    private bool isPlayer = false; //true = player, false = enemy

    private PlayerStats pS;
    private EnemyStats eS;

    private void Start()
    {
        try { pS = GetComponentInParent<PlayerStats>(); }
        catch { }

        try { eS = GetComponentInParent<EnemyStats>(); }
        catch { }

        if (pS != null) isPlayer = true;
        if (eS != null) isPlayer = false;
    }

    public int RollToHit()
    {
        int hitCount = Random.Range(1, 21);

        if (isPlayer)
        {
            hitCount += pS.proficiencyBonus;
            if (weaponType == eWeaponType.strength) hitCount += pS.GetStrengthMod();
            else if (weaponType == eWeaponType.reflex) hitCount += pS.GetReflexMod();
        }
        else
        {
            hitCount += eS.proficiencyBonus;
            if (weaponType == eWeaponType.strength) hitCount += eS.GetStrengthMod();
            else if (weaponType == eWeaponType.reflex) hitCount += eS.GetReflexMod();
        }

        return hitCount;
    }

    public int RollDamage()
    {
        int damage = Random.Range(1, diceType + 1);
        if (isPlayer)
        {
            if (weaponType == eWeaponType.strength) damage += pS.GetStrengthMod();
            else if (weaponType == eWeaponType.reflex) damage += pS.GetReflexMod();
        }
        else
        {
            if (weaponType == eWeaponType.strength) damage += eS.GetStrengthMod();
            else if (weaponType == eWeaponType.reflex) damage += eS.GetReflexMod();
        }

        return damage;
    }
}
