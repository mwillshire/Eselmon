using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PassTurnButton : MonoBehaviour
{
    private PlayerController pC;
    private CombatGridManager cGM;
    private void Start()
    {
        pC = FindFirstObjectByType<PlayerController>();
        cGM = FindFirstObjectByType<CombatGridManager>();
    }

    public void PassTheTurn()
    {
        pC.PassTurn();
    }

}
