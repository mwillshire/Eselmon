//This class will be added to later when I start having weapons with special effects 
//These methods will be called by the Weapon class when attacking or dealing damage, or anything else in the future
//These methods could also be empty which is what will happed in BasicAttack child class.

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Attack : Action
{
    public int damageDiceType;

    public virtual void AttackEffect()
    {

    }
    public virtual void DamageEffect()
    {
        
    }

    public override void TriggeringEffect(Vector3 location)
    {
        pC.mode = PlayerController.eMode.attack;
        cC.combatAction = CombatController.eCombatAction.weaponAttack;
        cC.GridMouseClick(location);
    }
}
