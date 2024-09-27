using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Move : Action
{
    public override void TriggeringEffect(Vector3 location)
    {
        cC.combatAction = CombatController.eCombatAction.move;
        cC.GridMouseClick(location);
    }
}
