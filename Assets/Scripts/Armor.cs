//This script will go on all armor prefabs

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Armor : MonoBehaviour
{
    public enum eArmorType { none, light, medium, heavy };

    [Header("Inscribed")]
    public eArmorType armorType;
    public int baseAC;

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

    public int ArmorClass()
    {
        int aC = baseAC;

        switch (armorType)
        {
            case eArmorType.none:
                if (isPlayer) aC += pS.GetReflexMod();
                else aC += eS.GetReflexMod();
                break;
            case eArmorType.light:
                if (isPlayer) aC += pS.GetReflexMod();
                else aC += eS.GetReflexMod();
                break;
            case eArmorType.medium:
                int mod = 0;
                if (isPlayer) mod = pS.GetReflexMod();
                else mod = eS.GetReflexMod();

                if (mod >= 2) aC += 2;
                else aC += mod;
                break;
            case eArmorType.heavy:
                break;
        }

        return aC;
    }
}
